import processing.core.*; 
import processing.xml.*; 

import java.applet.*; 
import java.awt.Dimension; 
import java.awt.Frame; 
import java.awt.event.MouseEvent; 
import java.awt.event.KeyEvent; 
import java.awt.event.FocusEvent; 
import java.awt.Image; 
import java.io.*; 
import java.net.*; 
import java.text.*; 
import java.util.*; 
import java.util.zip.*; 
import java.util.regex.*; 

public class spriterotator2 extends PApplet {

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

public int exrotate(){
  float aa=sq(img.width);
  float bb=sq(img.height);
  int cc=(int)(sqrt(aa+bb));
  return cc;
}

public void setup(){
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
zoom=PApplet.parseFloat(settings[5]);



saveit=new Button(550, 45, 60, 30, 1);
activetextBox=boxes[1]; boxes[1].active=1;

}

public void draw(){
  
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
    frames=PApplet.parseInt(boxes[2].value);
    rotation=radians((PApplet.parseFloat(360)/PApplet.parseFloat(frames)));
    float fc1=PApplet.parseFloat(boxes[3].value)/100; 
    float fc2=PApplet.parseFloat(boxes[4].value)/100; 
    scaling=fc1;
    cscale=(fc2-fc1)/frames; //scale increase)
   xw=PApplet.parseInt(exrotate()*max(fc1, fc2));
   int hframes=0;
     float hsqr=sqrt(frames);
     if(hsqr-PApplet.parseInt(hsqr)==0){
       hframes=PApplet.parseInt(hsqr);
     }//-======----------------if number is square, use it as frames by
    
     else{
         int hss=PApplet.parseInt(hsqr);
     for(int i=hss; i>0; i--){
     float fk=frames%i;
     if(fk==0){
       hframes=i;
       i=0;
     }
     }
     }
int vframes=PApplet.parseInt(frames/hframes);   
 
   int lt=PApplet.parseInt(frames/hframes);
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


public void keyPressed(){
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

public void mousePressed(){
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
  zoom=main.height/PApplet.parseFloat(400);
}
}

public void mouseWheel(int delta){
 
 zoom-=delta*.1f;
 if(zoom<.01f){zoom=.01f; }
}

public void mouseReleased(){
  mouseKey=new PVector(0, 0, 0);
}



class Button{
  PVector loc;
  PVector sizex;
  int id;
  int pos;
  PImage graphic;
  
  Button(float xx, float yy, float ex, float ey, int idz){
    loc=new PVector(xx, yy, 0);
    graphic=loadImage("savebutton1.png");
    sizex=new PVector(graphic.width, graphic.height, 0);
    id=idz;
    pos=0;
  }
  
  public void display(){
    pushStyle();
    noFill();
    stroke(255);
    image(graphic, loc.x, loc.y);
    if(abs(mouseX-loc.x)<sizex.x/2 && abs(mouseY-loc.y)<sizex.y/2){
      rect(loc.x, loc.y, graphic.width, graphic.height);
    }
    popStyle();

       
  }
  public void isclick(){
       if(abs(mouseX-loc.x)<sizex.x/2 && abs(mouseY-loc.y)<sizex.y/2){
         if(fname!=null && xname!=null){
       full.save(xname+".png");
         }
     }
  
  }
  
}

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
 
 public void display(){
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
 
 public int isClick(){
   int thisis=0;
   if(abs(mouseX-loc.x)<sizex.x/2 && abs(mouseY-loc.y)<sizex.y/2){
     thisis=1;
   }
   return thisis;
 }
 
   public void updateText(){
  value="";
  for(int i=0; i<count; i++){
   value+=sentence[i];}
  }
 
}
  static public void main(String args[]) {
    PApplet.main(new String[] { "--bgcolor=#F0F0F0", "spriterotator2" });
  }
}
