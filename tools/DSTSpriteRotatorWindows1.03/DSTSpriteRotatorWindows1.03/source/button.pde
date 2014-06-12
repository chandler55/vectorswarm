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
  
  void display(){
    pushStyle();
    noFill();
    stroke(255);
    image(graphic, loc.x, loc.y);
    if(abs(mouseX-loc.x)<sizex.x/2 && abs(mouseY-loc.y)<sizex.y/2){
      rect(loc.x, loc.y, graphic.width, graphic.height);
    }
    popStyle();

       
  }
  void isclick(){
       if(abs(mouseX-loc.x)<sizex.x/2 && abs(mouseY-loc.y)<sizex.y/2){
         if(fname!=null && xname!=null){
       full.save(xname+".png");
         }
     }
  
  }
  
}
