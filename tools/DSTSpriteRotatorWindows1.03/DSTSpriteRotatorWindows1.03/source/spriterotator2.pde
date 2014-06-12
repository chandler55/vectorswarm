/*PNG sprite Rotator/Scaler by DSTgames. Copyright 2010 DSTgames.com. All rights reserved.
Sprites use the counterclockwise rotation style (for Game Editor), where 0 is right, 90 is up, etc. etc);
*/
PGraphics main;
float zoom;
PImage img;
PVector pos;
int frames;
int framenum;
float rotation;
float scaling;
float cscale;
PVector imgsize;
String fname;
int xw;
int ft;
int m;
int n;
//char sentence[]=new char[120];
int xcount=0;
int doit=0;
PGraphics full;
PGraphics temp;
textBox boxes[]=new textBox[7];
textBox activetextBox;
Button saveit;
int nboxes=7;
String xname;
PImage bg;
int isrotate=0;
int pcount=0;
int wasr=0;
String settings[]=new String[10];
PVector sloc;
PVector tloc;
PVector mouseKey;
PVector mloc;

int exrotate(){
  float aa=sq(img.width);
  float bb=sq(img.height);
  int cc=(int)(sqrt(aa+bb));
  return cc;
}

void setup(){
  frameRate(30);

  tloc=new PVector(0, 0, 0);
  mouseKey=new PVector(0, 0, 0);
  mloc=new PVector(0, 0, 0);
bg=loadImage("spbg.png");
  addMouseWheelListener(new java.awt.event.MouseWheelListener() { 
    public void mouseWheelMoved(java.awt.event.MouseWheelEvent evt) { 
      mouseWheel(evt.getWheelRotation());
    }
  }
  );
    imageMode(CENTER);
    rectMode(CENTER);
    pos=new PVector(0, 0, 0);

  // int xw=img.width;
  //  int yw=img.height;
  //size(xw, xw, OPENGL);
size(640, 480, JAVA2D); 
    settings=loadStrings("config.ini");
  sloc=new PVector(width/2, height/2+50);

boxes[1]=(new textBox(150, 30, 120, 15, 1, "FileName:"));
boxes[2]=(new textBox(320, 30, 50, 15, 1, "nFrames:"));
boxes[3]=(new textBox(480, 30,50, 15, 1, "Scalefrom(%):"));
boxes[4]=(new textBox(480, 60, 50, 15, 1, "Scaleto(%):"));
boxes[5]=(new textBox(150, 60, 120, 15, 1, "ExportName:"));
boxes[6]=(new textBox(320, 60, 40, 15, 1, "Rotate(y/n):"));

boxes[2].value=settings[1];
boxes[3].value=settings[2];
boxes[4].value=settings[3];
boxes[6].value=settings[4];
zoom=float(settings[5]);



saveit=new Button(550, 45, 60, 30, 1);
activetextBox=boxes[1]; boxes[1].active=1;

}

void draw(){
  
 // noLoop();
 if(mouseKey.y==1){
   tloc.x=mouseX-sloc.x;
   tloc.y=mouseY-sloc.y;
 }
else if(mouseKey.y>1){
   sloc.x=mouseX-tloc.x;
   sloc.y=mouseY-tloc.y;}
//sloc.x=mloc.x;
//sloc.y=mloc.y;
   
image(bg, width/2, height/2);
pushStyle();
fill(255, 100);
text("Do not add .png to filenames.\n Press ENTER to apply \nRotation/Scaling.\nMousewheel zooms in&out.\nDo not press 'shift' when\nspecifying rotation y/n.\nMake sure to enter export filename\nbefore saving.\n Hold Right Mouse to drag, click \nCenter Mouse to reset view.", 15, 120);
popStyle();
fill(255, 255);
for(int i=1; i<nboxes; i++){
  if(boxes[i]!=null){
    boxes[i].display();
  }
}
int j=0; int k=0;
  if(doit==1){
        pos=new PVector(0, 0, 0);
    if(boxes[6].value.equals("Y") || boxes[6].value.equals("y")){isrotate=1;}
    else{isrotate=0;}
     xname=boxes[5].value;
    frames=int(boxes[2].value);
    rotation=radians((float(360)/float(frames)));
    float fc1=float(boxes[3].value)/100; 
    float fc2=float(boxes[4].value)/100; 
    scaling=fc1;
    cscale=(fc2-fc1)/frames; //scale increase)
   xw=int(exrotate()*max(fc1, fc2));
   int hframes=0;
     float hsqr=sqrt(frames);
     if(hsqr-int(hsqr)==0){
       hframes=int(hsqr);
     }//-======----------------if number is square, use it as frames by
    
     else{
         int hss=int(hsqr);
     for(int i=hss; i>0; i--){
     float fk=frames%i;
     if(fk==0){
       hframes=i;
       i=0;
     }
     }
     }
int vframes=int(frames/hframes);   
 
   int lt=int(frames/hframes);
   int ftt=hframes*lt;
   int fft=0;
/*(   if(ftt<frames){
   ft+=1;
   }
*/
  full=createGraphics(hframes*xw, vframes*xw, JAVA2D);
  full.beginDraw();
  //full.fill(255, 10);
  //full.rect(0, 0,full.width, full.height);
  
  
  for(int i=0; i<frames; i++){
  main=createGraphics(xw, xw, JAVA2D);
  main.beginDraw();

  main.noFill();
  pos.x+=img.width;
    if(i==frames-1 &&  boxes[4].value.equals("100")){
    scaling=1;
  }
      if(i==0 &&  boxes[3].value.equals("100")){
    scaling=1;
  }
  imgsize.x=img.width*scaling;
  imgsize.y=img.height*scaling;

  scaling+=cscale;
  main.imageMode(CENTER);
  main.pushMatrix();
  main.translate(main.width/2, main.height/2);
  if(isrotate==1){main.rotate(-pos.z);}
  pos.z+=rotation;
  main.image(img, 0, 0, imgsize.x, imgsize.y);
  main.popMatrix(); 
  main.endDraw();


  full.image(main, j*xw, k*xw);
    j++; if(j==hframes){j=0; k++;}
}//end loop


  full.endDraw();
 doit=0;
  }

  else {//loop();

  if(wasr==1){
    pcount++;

      image(full, sloc.x, sloc.y, full.width*zoom, full.height*zoom);
      pushStyle();
      noFill();
      stroke(200);
      rect(sloc.x, sloc.y, full.width*zoom, full.height*zoom);
      popStyle();
       // text(ft, 100, 100);
  }
}

saveit.display();
text("PNG Sprite Rotator & Scaler by DSTGAMES.com (c) 2010", 140, 470);
text("Zoom is "+zoom, 500, 450);
if(mouseKey.y>0){
  mouseKey.y=2;}
}


void keyPressed(){
  if(key=='q'){
    saveFrame("snapx.jpg");
  }
  if(activetextBox!=null){
  if(key!=8){
    if(key!=ENTER && key!=RETURN){
  activetextBox.sentence[activetextBox.count]=key;
  activetextBox.count++;}
  }
  else{if(activetextBox.count>0){activetextBox.count--;}}
  fname="";
  activetextBox.updateText();
if(key==ENTER || key==RETURN){
  wasr=1;
  zoom=1;
     fname=boxes[1].value;
     img=loadImage(fname+".png");
     imgsize=new PVector(img.width, img.height, 0);
     doit=1;
     settings[0]=str(frames);
     settings[1]=boxes[2].value;
     settings[2]=boxes[3].value;
     settings[3]=boxes[4].value;
     settings[4]=(boxes[6].value);
     settings[5]=str(zoom);
     settings[6]="isset";
     saveStrings("config.ini", settings);
}
  
}
}

void mousePressed(){
  if(mouseButton==LEFT){
    mouseKey.x=1;
  for(int i=1; i<nboxes; i++){
    if(boxes[i]!=null){
      boxes[i].active=0; boxes[i].onoff=0;
      if(boxes[i].isClick()==1){
        activetextBox=boxes[i];
        activetextBox.active=1;
      }
    }
  }
  saveit.isclick();
}
else if(mouseButton==RIGHT){
  mouseKey.y=1;
}
else if(mouseButton==CENTER){
  sloc.x=width/2;
  sloc.y=width/2+50;
  zoom=main.height/float(400);
}
}

void mouseWheel(int delta){
 
 zoom-=delta*.1;
 if(zoom<.01){zoom=.01; }
}

void mouseReleased(){
  mouseKey=new PVector(0, 0, 0);
}



