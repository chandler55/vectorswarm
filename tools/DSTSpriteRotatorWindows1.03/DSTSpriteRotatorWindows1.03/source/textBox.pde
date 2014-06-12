class textBox{
  String value;
  char sentence[]=new char[120];
  PVector loc;
  PVector ul;
  PVector sizex;
  int id;
  int pos;
  int onoff;
  int active=0;
  int count=0;
  String name;
  
  textBox(float xx, float yy, float ex, float ey, int idz, String mname){
    loc=new PVector(xx, yy, 0);
    sizex=new PVector(ex, ey, 0);
    ul=new PVector(xx-(ex/2), yy-(ey/2), 0);
    id=idz;
    value="";
    pos=0;
    name=mname;
  }
 
 void display(){
   int t=name.length();
   text(name, loc.x-sizex.x/2 - (t*7), loc.y+2);
   imageMode(CENTER);
   rectMode(CENTER);
   fill(255);
   text(value, loc.x-sizex.x/2+4, loc.y+2);
   pushStyle();
   noFill();
   stroke(155+(onoff*100));
   rect(loc.x, loc.y, sizex.x, sizex.y);
   if(active==1){


   
     if(frameCount%10==0){
       onoff=abs(onoff-1);
      
     }
   }
 }
 
 int isClick(){
   int thisis=0;
   if(abs(mouseX-loc.x)<sizex.x/2 && abs(mouseY-loc.y)<sizex.y/2){
     thisis=1;
   }
   return thisis;
 }
 
   void updateText(){
  value="";
  for(int i=0; i<count; i++){
   value+=sentence[i];}
  }
 
}
