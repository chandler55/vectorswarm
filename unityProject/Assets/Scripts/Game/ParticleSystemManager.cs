using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleSystemManager : MonoBehaviour
{
    [HideInInspector]
    public const int MAX_PARTICLES = 1000;

    private static ParticleSystemManager instance;
    public static ParticleSystemManager Instance
    {
        get
        {
            return instance;
        }
    }

    public enum ParticleType
    {
        None,
        Enemy,
        Bullet,
        IgnoreGravity,
    }

    public class GameParticle
    {
        public int index;
        public Vector2 Velocity;
        public Vector2 Position;
        public Color color;
        public float duration;
        public float angle;
        public float percentLife;
        public bool alive;
        public float size;
        public ParticleType type;

        public GameParticle()
        {
            index = -1;
            Velocity = Vector2.zero;
            Position = Vector2.zero;
            alive = false;
            duration = 1.0f;
            angle = 0.0f;
            percentLife = 1.0f;
            size = 1.0f;
            type = ParticleType.None;
        }

        public void UpdateParticle()
        {
            if ( alive )
            {
                Position += Velocity;

                if ( Mathf.Abs( Velocity.x ) + Mathf.Abs( Velocity.y ) < 0.00001f )
                    Velocity = Vector2.zero;

                Velocity *= 0.97f;

                Vector2 dir = ( Position + Velocity ) - (Vector2)Position;
                Vector2 normalized = Velocity.normalized;
                float angle2 = 360 - Mathf.Atan2( normalized.y, normalized.x ) * Mathf.Rad2Deg;

                float speed = Velocity.magnitude;
                float alpha = Mathf.Min( 1, Mathf.Min( percentLife * 2, speed * 1f ) );
                alpha *= alpha;

                color.a = alpha;

                //particle.Scale.x = particle.State.LengthMultiplier * Math.Min( Math.Min( 1f, 0.2f * speed + 0.1f ), alpha );
                //size = Mathf.Min( Mathf.Min( 1, 0.2f * speed + 0.1f ), alpha ) * 10.0f;
                size = Mathf.Min( 1, 0.2f * speed + 0.0f ) * 10.0f;

                percentLife -= ( 1f / duration ) * Time.deltaTime;

                if ( percentLife <= 0.0f )
                {
                    color = Color.clear;
                    alive = false;
                }

                // collide with the edges of the screen
                if ( Position.x < GameSettings.WORLD_BOUNDARY.x )
                {
                    Velocity.x = Mathf.Abs( Velocity.x );
                }
                else if ( Position.x > GameSettings.WORLD_BOUNDARY.x + GameSettings.WORLD_BOUNDARY.width )
                {
                    Velocity.x = -Mathf.Abs( Velocity.x );
                }

                if ( Position.y < GameSettings.WORLD_BOUNDARY.y )
                {
                    Velocity.y = Mathf.Abs( Velocity.y );
                }
                else if ( Position.y > GameSettings.WORLD_BOUNDARY.y + GameSettings.WORLD_BOUNDARY.height )
                {
                    Velocity.y = -Mathf.Abs( Velocity.y );
                }

                // black holes
                if ( type != ParticleType.IgnoreGravity )
                {
                    foreach ( BlackHole blackHole in BlackHole.blackHolesList )
                    {
                        var dPos = blackHole.Position - Position;
                        float distance = dPos.magnitude;
                        var n = dPos.normalized;

                        Velocity += ( 10000 * n / ( distance * distance + 10000 ) ) * Time.deltaTime;

                        // add tangential acceleration for nearby particles
                        if ( distance < 50 )
                            Velocity += ( 45 * new Vector2( n.y, -n.x ) / ( distance + 100 ) ) * Time.deltaTime;
                    }
                }

                ParticleSystemManager.UpdateParticle( index, Position, color, angle2, size );
            }
        }
    }

    private class CircularParticleArray
    {
        private int start;
        public int Start
        {
            get { return start; }
            set { start = value % list.Length; }
        }

        public int Count { get; set; }
        public int Capacity { get { return list.Length; } }
        private GameParticle[] list;

        public CircularParticleArray( int capacity )
        {
            list = new GameParticle[capacity];
        }

        public GameParticle this[int i]
        {
            get { return list[( start + i ) % list.Length]; }
            set { list[( start + i ) % list.Length] = value; }
        }
    }

    private ParticleSystem mParticleSystem = null;
    private static ParticleSystem.Particle[] mParticles;
    private bool mParticleSystemInitialized = false;
    private CircularParticleArray mGameParticleList;

    void Awake()
    {
        instance = this;
    }

    void OnDestroy()
    {
        instance = null;
    }

    void Start()
    {
        mParticleSystem = GetComponentInChildren<ParticleSystem>();
        mParticles = new ParticleSystem.Particle[MAX_PARTICLES];

        mGameParticleList = new CircularParticleArray( MAX_PARTICLES );
        for ( int i = 0; i < MAX_PARTICLES; i++ )
        {
            mGameParticleList[i] = new GameParticle();
            mGameParticleList[i].index = i;
        }
    }

    void Update()
    {
        if ( !mParticleSystemInitialized )
        {
            if ( mParticleSystem.particleCount >= MAX_PARTICLES )
            {
                mParticleSystemInitialized = true;
                mParticleSystem.GetParticles( mParticles );
            }
        }

        if ( mParticleSystemInitialized )
        {
            mParticleSystem.GetParticles( mParticles );

            // remove destroyed particles
            int removalCount = 0;
            for ( int i = 0; i < mGameParticleList.Count; i++ )
            {
                var particle = mGameParticleList[i];

                mGameParticleList[i].UpdateParticle();

                // sift deleted particles to the end of the list
                Swap( mGameParticleList, i - removalCount, i );

                // if the particle has expired, delete this particle
                if ( particle.percentLife < 0 )
                    removalCount++;
            }

            mGameParticleList.Count -= removalCount;

            mParticleSystem.SetParticles( mParticles, mParticleSystem.particleCount );
        }
    }

    private static void Swap( CircularParticleArray list, int index1, int index2 )
    {
        var temp = list[index1];
        list[index1] = list[index2];
        list[index2] = temp;
    }

    public void CreateParticle( Vector3 pos, Vector2 velocity, Color color, float duration, Vector2 scale, float angle )
    {
        GameParticle particle;
        if ( mGameParticleList.Count == mGameParticleList.Capacity )
        {
            // if the list is full, overwrite the oldest particle, and rotate the circular list
            particle = mGameParticleList[0];
            mGameParticleList.Start++;
        }
        else
        {
            particle = mGameParticleList[mGameParticleList.Count];
            mGameParticleList.Count++;
        }

        particle.Position = pos;
        particle.Velocity = velocity;
        particle.color = color;
        particle.alive = true;
        particle.angle = angle;
        particle.percentLife = 1.0f;
    }

    public static void UpdateParticle( int index, Vector3 pos, Color color, float angle, float size )
    {
        mParticles[index].position = pos;
        mParticles[index].velocity = Vector3.zero;
        mParticles[index].color = color;
        mParticles[index].rotation = angle;
        mParticles[index].size = size;
    }

    public void CreateEnemyExplosion( Vector3 pos )
    {
        float hue1 = Random.Range( 0, 6.0f );
        float hue2 = ( hue1 + Random.Range( 0, 2.0f ) ) % 6f;
        Color color1 = ColorUtil.HSVToColor( hue1, 0.5f, 1 );
        Color color2 = ColorUtil.HSVToColor( hue2, 0.5f, 1 );

        for ( int i = 0; i < 120; i++ )
        {
            float speed = 1f * ( 1f - 1 / Random.Range( 1f, 10f ) );
            Vector2 velocity = GameUtils.RandomVector2( 0, speed );
            Color color = Color.Lerp( color1, color2, Random.Range( 0, 1.0f ) );
            CreateParticle( pos, velocity, color, 3.0f, Vector2.one, 0 );
        }
    }

    public void CreateBulletExplosion( Vector3 pos )
    {
        for ( int i = 0; i < 30; i++ )
        {
            CreateParticle( pos, new Vector2( Random.Range( 0.0f, 1.0f ), Random.Range( 0.0f, 1.0f ) ), Color.blue, 1.0f, Vector2.one, 0 );
        }
    }

    public void CreatePlayerExplision( Vector3 pos )
    {
        Color yellow = new Color( 0.8f, 0.8f, 0.4f );

        for ( int i = 0; i < 500; i++ )
        {
            float speed = 1 * ( 1f - 1 / Random.Range( 1.0f, 10f ) );
            Color color = Color.Lerp( Color.white, yellow, Random.Range( 0, 1.0f ) );
            Vector2 velocity = GameUtils.RandomVector2( speed, speed );
            CreateParticle( pos, velocity, color, 3.0f, Vector2.one, 0 );
        }
    }
}
