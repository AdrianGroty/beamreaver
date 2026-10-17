/*{
  "ISFVSN": "2.0",
  "VSN": "1.1-geometry.2",
  "DESCRIPTION": "Polygons",
  "CREDIT": "codex",
  "CATEGORIES": [
    "Generator",
    "Geometry"
  ],
  "INPUTS": [
    {
      "NAME": "order",
      "LABEL": "Order",
      "TYPE": "float",
      "MIN": 1,
      "MAX": 99,
      "DEFAULT": 3
    },
    {
      "NAME": "depth",
      "LABEL": "Depth",
      "TYPE": "float",
      "MIN": 1,
      "MAX": 98,
      "DEFAULT": 6
    },
    {
      "NAME": "increment",
      "LABEL": "Recursion Increment",
      "TYPE": "float",
      "MIN": 1,
      "MAX": 98,
      "DEFAULT": 8
    },
    {
      "NAME": "fill",
      "LABEL": "Fill",
      "TYPE": "bool",
      "DEFAULT": false
    },
    {
      "NAME": "rotationSpeed",
      "LABEL": "Rotation Speed (cycles/sec)",
      "TYPE": "float",
      "MIN": -1,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "rotationInc",
      "LABEL": "Rotation Inc (radians/layer)",
      "TYPE": "float",
      "MIN": -1,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "rotationPhase",
      "LABEL": "Rotation Phase (0..1 = one cycle)",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "radiusAmount",
      "LABEL": "Radius Amount",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "radiusSpeed",
      "LABEL": "Radius Speed",
      "TYPE": "float",
      "MIN": -1,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "radiusInc",
      "LABEL": "Radius Inc",
      "TYPE": "float",
      "MIN": -1,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "radiusPhase",
      "LABEL": "Radius Phase (0..1 = one cycle)",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "geometryScale",
      "LABEL": "Geometry Scale",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 4,
      "DEFAULT": 1
    },
    {
      "NAME": "originX",
      "LABEL": "Origin X",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0.5
    },
    {
      "NAME": "originY",
      "LABEL": "Origin Y",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0.5
    },
    {
      "NAME": "bending",
      "LABEL": "Bend On",
      "TYPE": "bool",
      "DEFAULT": false
    },
    {
      "NAME": "horizontalBend",
      "LABEL": "Horizontal Bend",
      "TYPE": "float",
      "MIN": -1,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "verticalBend",
      "LABEL": "Vertical Bend",
      "TYPE": "float",
      "MIN": -1,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "bendMode",
      "LABEL": "Bend Mode",
      "TYPE": "long",
      "VALUES": [
        0,
        1,
        2,
        3
      ],
      "LABELS": [
        "Square",
        "Lens",
        "Sphere",
        "Twist"
      ],
      "DEFAULT": 0
    },
    {
      "NAME": "bendFunction",
      "LABEL": "Bend Function",
      "TYPE": "long",
      "VALUES": [
        0,
        1
      ],
      "LABELS": [
        "Light",
        "Heavy"
      ],
      "DEFAULT": 0
    },
    {
      "NAME": "viewZoom",
      "LABEL": "View Zoom",
      "TYPE": "float",
      "MIN": 0.5,
      "MAX": 2,
      "DEFAULT": 1
    },
    {
      "NAME": "pointSize",
      "LABEL": "Line Width (reference pixels)",
      "TYPE": "float",
      "MIN": 0.5,
      "MAX": 4,
      "DEFAULT": 1.5
    },
    {
      "NAME": "resetPhases",
      "LABEL": "Reset Accumulated Phases",
      "TYPE": "event"
    }
  ],
  "PASSES": [
    {
      "TARGET": "phaseState",
      "PERSISTENT": true,
      "WIDTH": "3",
      "HEIGHT": "1"
    },
    {
      "TARGET": "directions",
      "WIDTH": "200",
      "HEIGHT": "98"
    },
    {
      "TARGET": "agGeometryBounds",
      "WIDTH": "1",
      "HEIGHT": "1",
      "FLOAT": true
    },
    {}
  ]
}*/
#define TAU 6.283185307179586
#define STEP 1.2566370964050293
float countN(){return clamp(floor(order),1.0,99.0);}
float countD(){return clamp(floor(depth),1.0,98.0);}
float spacing(){return 2.0*clamp(increment,1.0,98.0)*(clamp(geometryScale,0.0,4.0)+.5);}
int scans(){return fill?int(floor(spacing())):1;}
vec2 originPoint(){return vec2(960.0*originX,640.0*(1.0-originY));}
vec2 truncPoint(vec2 p){return sign(p)*floor(abs(p));}
float rotationAdjust(float x){if(x>=TAU)return abs(x-TAU);if(x<0.0)return TAU-abs(x);return x;}
vec4 packFloat(float value){
 float a=abs(value);if(!(a>0.0))return vec4(0.0);
 float e=floor(log2(a)),power=exp2(e);
 if(a<power){e-=1.0;power*=.5;}
 if(a>=power*2.0){e+=1.0;power*=2.0;}
 float m=floor((a/power-1.0)*8388608.0+.5);
 if(m>=8388608.0){m=0.0;e+=1.0;}
 float exponent=e+127.0;
 return vec4((value<0.0?128.0:0.0)+floor(exponent*.5),mod(exponent,2.0)*128.0+floor(m/65536.0),mod(floor(m/256.0),256.0),mod(m,256.0))/255.0;
}
float unpackFloat(vec4 pixel){
 vec4 b=floor(pixel*255.0+.5);if(dot(b,vec4(1.0))==0.0)return 0.0;
 float e=mod(b.r,128.0)*2.0+floor(b.g/128.0)-127.0;
 float m=mod(b.g,128.0)*65536.0+b.b*256.0+b.a;
 return (b.r>=128.0?-1.0:1.0)*(1.0+m/8388608.0)*exp2(e);
}
vec3 clockValue(){
 return vec3(unpackFloat(IMG_NORM_PIXEL(phaseState,vec2(1.0/6.0,.5))),
             unpackFloat(IMG_NORM_PIXEL(phaseState,vec2(.5,.5))),
             unpackFloat(IMG_NORM_PIXEL(phaseState,vec2(5.0/6.0,.5))));
}

float angleStep(){int n=int(countN());
if(n<=50){if(n<=25){if(n<=13){if(n<=7){if(n<=4){if(n<=2){if(n<=1){return 6.2831854820251465;}else{return 3.1415927410125732;}}else{if(n<=3){return 2.0943951606750488;}else{return 1.5707963705062866;}}}else{if(n<=6){if(n<=5){return 1.2566370964050293;}else{return 1.0471975803375244;}}else{return 0.89759790897369385;}}}else{if(n<=10){if(n<=9){if(n<=8){return 0.78539818525314331;}else{return 0.69813168048858643;}}else{return 0.62831854820251465;}}else{if(n<=12){if(n<=11){return 0.57119864225387573;}else{return 0.52359879016876221;}}else{return 0.48332193493843079;}}}}else{if(n<=19){if(n<=16){if(n<=15){if(n<=14){return 0.44879895448684692;}else{return 0.41887903213500977;}}else{return 0.39269909262657166;}}else{if(n<=18){if(n<=17){return 0.36959913372993469;}else{return 0.34906584024429321;}}else{return 0.33069396018981934;}}}else{if(n<=22){if(n<=21){if(n<=20){return 0.31415927410125732;}else{return 0.29919931292533875;}}else{return 0.28559932112693787;}}else{if(n<=24){if(n<=23){return 0.2731819748878479;}else{return 0.2617993950843811;}}else{return 0.25132742524147034;}}}}}else{if(n<=38){if(n<=32){if(n<=29){if(n<=27){if(n<=26){return 0.24166096746921539;}else{return 0.2327105700969696;}}else{if(n<=28){return 0.22439947724342346;}else{return 0.21666155755519867;}}}else{if(n<=31){if(n<=30){return 0.20943951606750488;}else{return 0.20268340408802032;}}else{return 0.19634954631328583;}}}else{if(n<=35){if(n<=34){if(n<=33){return 0.19039955735206604;}else{return 0.18479956686496735;}}else{return 0.17951957881450653;}}else{if(n<=37){if(n<=36){return 0.17453292012214661;}else{return 0.16981582343578339;}}else{return 0.16534698009490967;}}}}else{if(n<=44){if(n<=41){if(n<=40){if(n<=39){return 0.16110731661319733;}else{return 0.15707963705062866;}}else{return 0.15324842929840088;}}else{if(n<=43){if(n<=42){return 0.14959965646266937;}else{return 0.1461205929517746;}}else{return 0.14279966056346893;}}}else{if(n<=47){if(n<=46){if(n<=45){return 0.13962633907794952;}else{return 0.13659098744392395;}}else{return 0.13368479907512665;}}else{if(n<=49){if(n<=48){return 0.13089969754219055;}else{return 0.12822827696800232;}}else{return 0.12566371262073517;}}}}}}else{if(n<=75){if(n<=63){if(n<=57){if(n<=54){if(n<=52){if(n<=51){return 0.1231997087597847;}else{return 0.1208304837346077;}}else{if(n<=53){return 0.11855066567659378;}else{return 0.1163552850484848;}}}else{if(n<=56){if(n<=55){return 0.11423972994089127;}else{return 0.11219973862171173;}}else{return 0.11023131757974625;}}}else{if(n<=60){if(n<=59){if(n<=58){return 0.10833077877759933;}else{return 0.10649466514587402;}}else{return 0.10471975803375244;}}else{if(n<=62){if(n<=61){return 0.10300303995609283;}else{return 0.10134170204401016;}}else{return 0.099733099341392517;}}}}else{if(n<=69){if(n<=66){if(n<=65){if(n<=64){return 0.098174773156642914;}else{return 0.096664391458034515;}}else{return 0.09519977867603302;}}else{if(n<=68){if(n<=67){return 0.093778885900974274;}else{return 0.092399783432483673;}}else{return 0.091060653328895569;}}}else{if(n<=72){if(n<=71){if(n<=70){return 0.089759789407253265;}else{return 0.088495567440986633;}}else{return 0.087266460061073303;}}else{if(n<=74){if(n<=73){return 0.086071029305458069;}else{return 0.084907911717891693;}}else{return 0.083775803446769714;}}}}}else{if(n<=87){if(n<=81){if(n<=78){if(n<=77){if(n<=76){return 0.082673490047454834;}else{return 0.081599809229373932;}}else{return 0.080553658306598663;}}else{if(n<=80){if(n<=79){return 0.079533994197845459;}else{return 0.078539818525314331;}}else{return 0.077570192515850067;}}}else{if(n<=84){if(n<=83){if(n<=82){return 0.076624214649200439;}else{return 0.075701028108596802;}}else{return 0.074799828231334686;}}else{if(n<=86){if(n<=85){return 0.073919825255870819;}else{return 0.073060296475887299;}}else{return 0.072220519185066223;}}}}else{if(n<=93){if(n<=90){if(n<=89){if(n<=88){return 0.071399830281734467;}else{return 0.070597589015960693;}}else{return 0.069813169538974762;}}else{if(n<=92){if(n<=91){return 0.069045990705490112;}else{return 0.068295493721961975;}}else{return 0.067561134696006775;}}}else{if(n<=96){if(n<=95){if(n<=94){return 0.066842399537563324;}else{return 0.066138789057731628;}}else{return 0.065449848771095276;}}else{if(n<=98){if(n<=97){return 0.06477510929107666;}else{return 0.06411413848400116;}}else{return 0.063466519117355347;}}}}}}
}
void clockPass(){
 vec3 old=clockValue();bool reset=FRAMEINDEX==0 || resetPhases;if(reset)old=vec3(0.0);
 float dt=reset?0.0:max(TIMEDELTA,0.0);
 float next=old.b+countN()*countD()*float(scans())*(STEP/TAU)*radiusSpeed*25.0*dt;
 vec3 value=vec3(mod(old.r+rotationSpeed*dt,1.0),old.g+floor(next),mod(next,1.0));
 float x=floor(gl_FragCoord.x);gl_FragColor=packFloat(x<.5?value.r:(x<1.5?value.g:value.b));
}
float layerPhi(float layer,vec3 clock){return rotationAdjust(TAU*mod(clock.r+rotationPhase,1.0)+layer*rotationInc);}
vec2 calculatedUnit(int index,int layer){
 float angle=layerPhi(float(layer),clockValue()),delta=angleStep();
 for(int k=0;k<99;k++){if(k>=index)break;angle+=delta;}
 return vec2(cos(angle),sin(angle));
}
vec2 unitAt(int index,int layer){
 float x=float(index)*2.0;float y=(float(layer)+.5)/98.0;
 return vec2(unpackFloat(IMG_NORM_PIXEL(directions,vec2((x+.5)/200.0,y))),
             unpackFloat(IMG_NORM_PIXEL(directions,vec2((x+1.5)/200.0,y))));
}
float layerScale(int layer,int sub){return 1.0+float(layer)*spacing()+(fill?float(sub)+1.0:0.0);}
float modAmount(int side,int layer,int sub,vec3 clock){
 if(radiusAmount==0.0)return 0.0;
 int invocation=layer*scans()+sub;
 float stepCycles=(STEP/TAU)*radiusSpeed;
 float spatial=float(side)*(1.0/countN())*(20.0*radiusInc)/TAU;
 float partial=float(side+1)*stepCycles+spatial+mod(radiusPhase,1.0);
 float ringCycles=float(invocation)*(countN()*stepCycles);
 float signPhase=clock.g+clock.b+ringCycles+partial;
 float phase=clock.b+mod(ringCycles,1.0)+partial;
 float wave=sin(TAU*mod(phase,1.0));if(signPhase<0.0)wave=-wave;
 return radiusAmount*(1.0+.5*wave);
}
vec4 segment(int side,int layer,int sub,vec3 clock){
 float s=layerScale(layer,sub),amount=modAmount(side,layer,sub,clock);
 // Native multiply-add order: S*m+S (rather than one S*gain multiply).
 float radius=s*amount+s;
 vec2 o=originPoint();return vec4(truncPoint(o+radius*unitAt(side,layer)),truncPoint(o+radius*unitAt(side+1,layer)));
}
float lineCoverage(vec2 p,vec4 s,float aa){
 vec2 v=s.zw-s.xy;
 float t=clamp(dot(p-s.xy,v)/max(dot(v,v),1.e-12),0.0,1.0);
 float distance=length(p-s.xy-t*v)-pointSize*.5;
 return 1.0-smoothstep(-aa,aa,distance);
}
int wrappedSide(int value,int n){int r=value-(value/n)*n;return r<0?r+n:r;}
float coverage(vec2 p,float aa){
 vec2 origin=originPoint(),z=p-origin;float rho=length(z),b=spacing(),n=countN();int ni=int(n),f=scans();
 float margin=pointSize*.5+aa+1.6;
 float lowGain=1.0+.5*radiusAmount,highGain=1.0+1.5*radiusAmount;
 float halfCos=ni==1?1.0:(ni==2?0.0:cos(3.141592653589793/n));
 float minScale=max(0.0,(rho-margin)/highGain);
 float maxScale=halfCos>1.e-7?(rho+margin)/(halfCos*lowGain):1.e9;
 float offset=fill?2.0:1.0;
 int firstLayer=max(0,int(ceil((minScale-offset-(fill?float(f-1):0.0))/b))-1);
 int lastLayer=min(int(countD())-1,int(floor((maxScale-offset)/b))+1);
 float result=0.0;vec3 clock=clockValue();
 float delta=angleStep(),polar=atan(z.y,z.x);
 float span=asin(clamp(margin/max(rho,1.e-12),0.0,1.0))/delta;
 for(int visit=0;visit<98;visit++){
  int layer=firstLayer+visit;if(layer>lastLayer)break;
  int firstSide=0,sideCount=ni;
  if(ni>=3 && rho>margin){
   float center=mod(polar-layerPhi(float(layer),clock),TAU)/delta;
   firstSide=int(floor(center-span))-1;
   sideCount=min(ni,int(floor(center+span))-firstSide+2);
  }
  for(int k=0;k<99;k++){
   if(k>=sideCount)break;int side=wrappedSide(firstSide+k,ni);
   vec2 a=unitAt(side,layer),c=unitAt(side+1,layer);
   float s0=layerScale(layer,0),s1=layerScale(layer,f-1);
   float sideLow=lowGain,sideHigh=highGain;
   if(radiusSpeed==0.0){sideLow=1.0+modAmount(side,layer,0,clock);sideHigh=sideLow;}
   float r0=s0*sideLow,r1=s1*sideHigh;
   vec2 lo=min(min(a*r0,a*r1),min(c*r0,c*r1))-vec2(margin);
   vec2 hi=max(max(a*r0,a*r1),max(c*r0,c*r1))+vec2(margin);
   if(any(lessThan(z,lo)) || any(greaterThan(z,hi)))continue;
   int firstSub=0,lastSub=f-1;
   if(fill){
    vec2 sum=a+c;float norm=length(sum);
    if(ni>=3 && norm>1.e-6){
     vec2 normal=sum/norm;float h=dot(a,normal),parallel=dot(z,normal);
     float lower=max(0.0,(parallel-margin)/h),upper=(parallel+margin)/h;
     firstSub=max(0,int(ceil(lower/sideHigh-s0))-1);
     lastSub=min(f-1,int(floor(upper/sideLow-s0))+1);
    }else{
     firstSub=max(0,int(ceil(minScale-s0))-1);
     if(ni==1)lastSub=min(f-1,int(floor(maxScale-s0))+1);
     if(ni==2 && abs(z.x*a.y-z.y*a.x)>margin)continue;
    }
   }
   for(int h=0;h<882;h++){
    int sub=firstSub+h;if(sub>lastSub)break;
    float scale=layerScale(layer,sub),amount=modAmount(side,layer,sub,clock);
    float radius=scale*amount+scale;
    vec4 endpoints=vec4(truncPoint(origin+radius*a),truncPoint(origin+radius*c));
    result=max(result,lineCoverage(p,endpoints,aa));
    if(result>=1.0)return result;
   }
  }
 }
 return result;
}
vec4 agCalculateBounds(){vec4 box=vec4(1.e20,1.e20,-1.e20,-1.e20);float r=layerScale(int(countD())-1,scans()-1)*(1.0+1.5*radiusAmount);vec2 o=originPoint();box=vec4(o-vec2(r),o+vec2(r));return box+vec4(-pointSize-4.0,-pointSize-4.0,pointSize+4.0,pointSize+4.0);}

#define AG_SOURCE_COVERAGE coverage
// Shared Arabesque Geometry panel. Reference coordinates have Y down.
// Recovered from dispatcher 0x460a78..0x460b15 and point helper 0x459470.
// Continuous coverage replaces the native integer scatter rasterization.
vec2 agBend(vec2 p) {
    vec2 size=vec2(960.0,640.0),center=size*.5;
    vec2 amount=vec2(horizontalBend,verticalBend);
    float gain=bendFunction==0?1.0:5.0;
    if(bendMode==0) return p+(p-center)*amount*(bendFunction==0?1.0:10.0);
    if(bendMode==1) return p+sin((p-center)/size*1.570796326794897)*size*.5*amount*gain;
    vec2 wave=vec2(bendMode==3?cos(p.y/size.y*3.141592653589793):sin(p.y/size.y*3.141592653589793),sin(p.x/size.x*3.141592653589793));
    return p+wave*size*.5*amount*gain;
}
// Columns of the analytic forward Jacobian.
mat2 agJacobian(vec2 p) {
    vec2 size=vec2(960.0,640.0),amount=vec2(horizontalBend,verticalBend);
    float gain=bendFunction==0?1.0:5.0;
    if(bendMode==0){vec2 d=1.0+amount*(bendFunction==0?1.0:10.0);return mat2(d.x,0.0,0.0,d.y);}
    if(bendMode==1){vec2 d=1.0+cos((p-size*.5)/size*1.570796326794897)*amount*(.7853981633974483*gain);return mat2(d.x,0.0,0.0,d.y);}
    float dxdy=(bendMode==3?-sin(p.y/640.0*3.141592653589793):cos(p.y/640.0*3.141592653589793))*amount.x*(.75*3.141592653589793*gain);
    float dydx=cos(p.x/960.0*3.141592653589793)*amount.y*(3.141592653589793/3.0*gain);
    return mat2(1.0,dydx,dxdy,1.0);
}
vec2 agSolve(mat2 j,vec2 v) {
    float det=j[0].x*j[1].y-j[1].x*j[0].y;
    if(abs(det)<.00001)det=det<0.0?-.00001:.00001;
    return vec2(j[1].y*v.x-j[1].x*v.y,-j[0].y*v.x+j[0].x*v.y)/det;
}
float agCoverageAt(vec2 p,float aa){return AG_SOURCE_COVERAGE(p,aa);}
vec4 agSourceBounds(float aa){
    vec2 uv=vec2(.5);
    vec4 box=IMG_NORM_PIXEL(agGeometryBounds,uv);
    return box+vec4(-aa*100.0,-aa*100.0,aa*100.0,aa*100.0);
}
vec2 agWaveRange(float lo,float hi,bool cosine){
    if(cosine){lo+=1.570796326794897;hi+=1.570796326794897;}
    vec2 r=vec2(min(sin(lo),sin(hi)),max(sin(lo),sin(hi)));
    if(ceil((lo-1.570796326794897)/6.283185307179586)<=floor((hi-1.570796326794897)/6.283185307179586))r.y=1.0;
    if(ceil((lo+1.570796326794897)/6.283185307179586)<=floor((hi+1.570796326794897)/6.283185307179586))r.x=-1.0;
    return r;
}
// Eliminate y from the coupled Sphere/Twist equations. Every inverse point
// satisfies y=target.y-B*sin(pi*x/960), leaving a bounded scalar equation.
float agCoupledValue(float x,vec2 target,vec2 amount){
    float y=target.y-amount.y*sin(x*.003272492347489368);
    float angle=y*.004908738521234052;
    return x+amount.x*(bendMode==3?cos(angle):sin(angle))-target.x;
}
vec2 agCoupledValueDerivative(float x,vec2 target,vec2 amount){
    float ax=x*.003272492347489368;
    float y=target.y-amount.y*sin(ax);
    float ay=y*.004908738521234052;
    float wave=bendMode==3?cos(ay):sin(ay);
    float derivative=bendMode==3?-sin(ay):cos(ay);
    return vec2(x+amount.x*wave-target.x,1.0-amount.x*amount.y*.00001606380924656471*derivative*cos(ax));
}
float agCoupledCoverage(vec2 target,float aa){
    vec2 amount=vec2(horizontalBend*2400.0,verticalBend*1600.0);
    if(bendFunction==0)amount*=.2;
    // The original search runs 25 independent two-dimensional Newton solves.
    // Scan the exact bounded scalar domain, then refine only sign brackets.
    // Cost follows bend strength; the search does not evaluate source geometry
    // for failed seeds or rediscover the same root from different seeds.
    vec4 box=agSourceBounds(aa);
    if(box.x>box.z || box.y>box.w)return 0.0;
    vec2 xr=agWaveRange(box.y*.004908738521234052,box.w*.004908738521234052,bendMode==3)*amount.x;
    vec2 yr=agWaveRange(box.x*.003272492347489368,box.z*.003272492347489368,false)*amount.y;
    if(target.x<box.x+min(xr.x,xr.y) || target.x>box.z+max(xr.x,xr.y) || target.y<box.y+min(yr.x,yr.y) || target.y>box.w+max(yr.x,yr.y))return 0.0;
    float start=max(target.x-abs(amount.x)-.001,box.x),finish=min(target.x+abs(amount.x)+.001,box.z);
    if(start>=finish)return 0.0;
    int intervals=int(clamp(ceil((finish-start)/20.0),16.0,256.0));
    float low=start;
    float step=(finish-start)/float(intervals);
    float fLow=agCoupledValue(low,target,amount),result=0.0;
    for(int i=0;i<256;i++){
        if(i>=intervals)break;
        float high=start+float(i+1)*step;
        float fHigh=agCoupledValue(high,target,amount);
        if(fLow*fHigh<=0.0){
            float left=low,right=high,fl=fLow;
            float x=(left+right)*.5;
            for(int it=0;it<18;it++){
                vec2 fd=agCoupledValueDerivative(x,target,amount);
                if(abs(fd.x)<.0001)break;
                if(fl*fd.x<=0.0)right=x;else{left=x;fl=fd.x;}
                float next=x-fd.x/(abs(fd.y)>.00001?fd.y:.00001);
                x=(next>left && next<right)?next:(left+right)*.5;
            }
            if(abs(agCoupledValue(x,target,amount))<.01){
                vec2 p=vec2(x,target.y-amount.y*sin(x*.003272492347489368));
                if(p.y<box.y || p.y>box.w){low=high;fLow=fHigh;continue;}
                mat2 j=agJacobian(p);
                float stretch=max(length(agSolve(j,vec2(1.0,0.0))),length(agSolve(j,vec2(0.0,1.0))));
                result=max(result,agCoverageAt(p,aa*clamp(stretch,.01,100.0)));
                if(result>=1.0)return result;
            }
        }
        low=high;fLow=fHigh;
    }
    return result;
}
float agGeometryCoverage(vec2 target,float aa) {
    if(!bending || (horizontalBend==0.0 && verticalBend==0.0))return agCoverageAt(target,aa);
    if(bendMode==0){
        vec2 d=1.0+vec2(horizontalBend,verticalBend)*(bendFunction==0?1.0:10.0);
        if(abs(d.x)<.0001)d.x=d.x<0.0?-.0001:.0001;
        if(abs(d.y)<.0001)d.y=d.y<0.0?-.0001:.0001;
        return agCoverageAt((target-vec2(480.0,320.0))/d+vec2(480.0,320.0),aa/min(abs(d.x),abs(d.y)));
    }
    // Search multiple inverse branches: strong signed bends can fold strokes.
    // Coverage remains analytic; the extra 1x1 bounds cache never resamples
    // the image or updates clocks. Existing Lens/unique-map behavior stays.
    vec2 amplitude=abs(vec2(horizontalBend,verticalBend))*vec2(480.0,320.0)*(bendFunction==0?1.0:5.0);
    float gain=bendFunction==0?1.0:5.0;
    bool unique=bendMode==1 ? max(abs(horizontalBend),abs(verticalBend))*.7853981633974483*gain<.99 : abs(horizontalBend*verticalBend)*2.46740110027234*gain*gain<.99;
    if(bendMode>=2 && !unique)return agCoupledCoverage(target,aa);
    vec2 roots[25];int count=0;float result=0.0;
    for(int sy=0;sy<5;sy++)for(int sx=0;sx<5;sx++){
        vec2 p=target+amplitude*(vec2(float(sx),float(sy))*.5-1.0);
        if(sx==0 && sy==0)p=target;
        for(int it=0;it<24;it++){
            vec2 error=agBend(p)-target;
            if(dot(error,error)<.000001)break;
            vec2 step=agSolve(agJacobian(p),error);
            // Damping keeps near-singular Newton steps inside the search region.
            float factor=max(1.0,max(abs(step.x)/(amplitude.x+1.0),abs(step.y)/(amplitude.y+1.0)));
            p-=step/factor;
        }
        if(length(agBend(p)-target)>.01)continue;
        bool duplicate=false;
        for(int k=0;k<25;k++){if(k>=count)break;if(length(p-roots[k])<.05)duplicate=true;}
        if(duplicate)continue;
        roots[count]=p;count++;
        mat2 j=agJacobian(p);
        float stretch=max(length(agSolve(j,vec2(1.0,0.0))),length(agSolve(j,vec2(0.0,1.0))));
        result=max(result,agCoverageAt(p,aa*clamp(stretch,.01,100.0)));
        if(unique)return result;
        if(result>=1.0)return result;
    }
    return result;
}

void agOriginalMain(){
 if(PASSINDEX==0){clockPass();return;}
 if(PASSINDEX==1){
  int x=int(floor(gl_FragCoord.x)),index=x/2,component=x-index*2,layer=int(floor(gl_FragCoord.y));
  vec2 u=calculatedUnit(index,layer);gl_FragColor=packFloat(component==0?u.x:u.y);return;
 }
 float fit=min(RENDERSIZE.x/960.0,RENDERSIZE.y/640.0)*viewZoom;
 vec2 p=(vec2(gl_FragCoord.x,RENDERSIZE.y-gl_FragCoord.y)-RENDERSIZE*.5)/fit+vec2(480.0,320.0);
 float c=agGeometryCoverage(p,.55/fit);gl_FragColor=c>=1.0/255.0?vec4(vec3(1.0),c):vec4(0.0);
}

void main(){if(PASSINDEX==2){gl_FragColor=agCalculateBounds();return;}agOriginalMain();}
