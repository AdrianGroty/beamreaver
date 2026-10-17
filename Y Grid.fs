/*{
  "ISFVSN": "2.0",
  "VSN": "1.0-geometry.2",
  "DESCRIPTION": "Y Grid",
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
      "NAME": "rotationSpeed",
      "LABEL": "Rotation Speed (native units; 25 redraws/sec)",
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
      "LABEL": "Radius Amount (grid convergence)",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0
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
      "LABEL": "Reset Accumulated Rotation",
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
      "TARGET": "geometry",
      "WIDTH": "2000",
      "HEIGHT": "4"
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

int gridN(){return int(floor(9.6*clamp(floor(order),1.0,99.0)));}
int gridD(){return int(clamp(floor(depth),1.0,98.0));}
float spacing(){return clamp(increment,1.0,98.0);}
vec2 originPoint(){return vec2(float(int(960.0*originX)),float(int(640.0*(1.0-originY))));}
vec2 truncPoint(vec2 p){return sign(p)*floor(abs(p));}
void clockPass(){
 float phase=clockValue().r;
 bool reset=FRAMEINDEX==0 || resetPhases;
 if(reset)phase=0.0;
 else phase=mod(phase+(6.0*rotationSpeed)*(25.0/TAU)*max(TIMEDELTA,0.0),1.0);
 gl_FragColor=packFloat(gl_FragCoord.x<1.0?phase:0.0);
}
float rotationValue(){return mod(clockValue().r+rotationPhase,1.0);}
float taper(){
 float a=clamp(radiusAmount,0.0,1.0);
 // Native zero is a special case; the nonzero limit is one, not zero.
 if(a==0.0)return 0.0;
 if(a==1.0)return float(gridN());
 return pow(float(gridN()),a*a);
}
vec4 calculatedSegment(int index){
 int n=gridN(),d=gridD();float b=spacing(),q=taper();vec2 o=originPoint();
 float left=o.x-float(n)*b,right=o.x+float(n)*b,base=o.y+320.0;
 float height=float(d)*b;
 if(index<=2*n){
  float lowerLeft=left+height*q,lowerRight=right-height*q;
  float du=(right-left)/float(2*n),dv=(lowerRight-lowerLeft)/float(2*n);
  float u=left,v=lowerLeft;
  for(int k=0;k<1900;k++){
   if(k>=index)break;u+=du;v+=dv;
  }
  return vec4(truncPoint(vec2(u,base)),truncPoint(vec2(v,base-height)));
 }
 int j=index-2*n;float cursor=base;
 for(int k=0;k<98;k++){
  if(k>=j)break;cursor-=b;
 }
 float phase=rotationValue(),y=b*phase+cursor;
 float inset=(float(j)*b)*q,shift=(b*phase)*q;
 return vec4(truncPoint(vec2((left+inset)-shift,y)),
             truncPoint(vec2((right+shift)-inset,y)));
}
// Retain measured fan cursor deviations from the endpoint interpolant in
// unused cache column 1999. This keeps strong taper from inflating every
// output pixel's candidate range with a worst-case accumulation estimate.
vec2 calculatedFanError(){
 int n=gridN();float b=spacing(),q=taper();vec2 o=originPoint();
 float left=o.x-float(n)*b,right=o.x+float(n)*b,height=float(gridD())*b;
 float lowerLeft=left+height*q,lowerRight=right-height*q;
 float du=(right-left)/float(2*n),dv=(lowerRight-lowerLeft)/float(2*n);
 vec4 last=calculatedSegment(2*n);
 vec2 first=truncPoint(vec2(left,lowerLeft)),end=vec2(last.x,last.z);
 vec2 cursor=vec2(left,lowerLeft),error=vec2(0.0);
 for(int k=0;k<1901;k++){
  if(k>2*n)break;
  vec2 expected=first+(end-first)*(float(k)/float(2*n));
  error=max(error,abs(truncPoint(cursor)-expected));cursor+=vec2(du,dv);
 }
 float magnitude=max(max(abs(first.x),abs(first.y)),max(abs(end.x),abs(end.y)));
 return error+vec2(4.0+3.0*magnitude*1.1920928955078125e-7);
}
vec4 segmentAt(int index){
 float x=(float(index)+.5)/2000.0;
 return vec4(unpackFloat(IMG_NORM_PIXEL(geometry,vec2(x,.125))),
             unpackFloat(IMG_NORM_PIXEL(geometry,vec2(x,.375))),
             unpackFloat(IMG_NORM_PIXEL(geometry,vec2(x,.625))),
             unpackFloat(IMG_NORM_PIXEL(geometry,vec2(x,.875))));
}
float lineCoverage(vec2 p,vec4 s,float aa){
 vec2 v=s.zw-s.xy,w=p-s.xy;float t=dot(w,v)/max(dot(v,v),1.e-12);
 float distance=t<=0.0?length(w):(t>=1.0?length(p-s.zw):abs(v.x*w.y-v.y*w.x)/max(length(v),1.e-12));
 return 1.0-smoothstep(-aa,aa,distance-pointSize*.5);
}
float coverage(vec2 p,float aa){
 int d=gridD(),n=gridN();float b=spacing(),margin=pointSize*.5+aa+2.0,result=0.0;
 float base=originPoint().y+320.0;
 float verticalDrift=float(d)*float(d)*b*1.1920928955078125e-7;
 if(p.y>base+margin || p.y<base-float(d)*b-margin-verticalDrift)return 0.0;
 // Rotation shifts horizontal rows, not the stationary fan.
 float row=(base-p.y)/b+rotationValue();
 int firstRow=max(1,int(ceil(row-margin/b))-1),lastRow=min(d-1,int(floor(row+margin/b))+1);
 for(int j=0;j<97;j++){
  int index=firstRow+j;if(index>lastRow)break;
  result=max(result,lineCoverage(p,segmentAt(2*n+index),aa));if(result>=1.0)return result;
 }
 vec4 a=segmentAt(0),z=segmentAt(2*n);
 float t=clamp((p.y-a.y)/(a.w-a.y),0.0,1.0);
 float x0=mix(a.x,a.z,t),x1=mix(z.x,z.z,t),step=(x1-x0)/float(2*n);
 float errorU=unpackFloat(IMG_NORM_PIXEL(geometry,vec2(1999.5/2000.0,.125)));
 float errorV=unpackFloat(IMG_NORM_PIXEL(geometry,vec2(1999.5/2000.0,.375)));
 float drift=mix(errorU,errorV,t),height=abs(a.w-a.y);
 float directionStep=((z.z-z.x)-(a.z-a.x))/float(2*n);
 int first=0,last=2*n;
 float denominator=abs(step)-margin*abs(directionStep)/height;
 if(denominator>1.e-7){
  float center=(p.x-x0)/step;
  float localSlope=abs((a.z-a.x)+center*directionStep)/height;
  float reach=margin*(1.0+localSlope+(errorU+errorV)/height)+drift;
  float span=reach/denominator;
  first=max(0,int(ceil(center-span))-2);last=min(last,int(floor(center+span))+2);
 }
 for(int k=0;k<1901;k++){
  int i=first+k;if(i>last)break;
  result=max(result,lineCoverage(p,segmentAt(i),aa));if(result>=1.0)return result;
 }
 return result;
}
vec4 agCalculateBounds(){vec4 box=vec4(1.e20,1.e20,-1.e20,-1.e20);for(int i=0;i<2000;i++){if(i>=gridD()+2*gridN())break;vec4 s=segmentAt(i);box=vec4(min(box.xy,min(s.xy,s.zw)),max(box.zw,max(s.xy,s.zw)));}return box+vec4(-pointSize-4.0,-pointSize-4.0,pointSize+4.0,pointSize+4.0);}

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
  int i=int(floor(gl_FragCoord.x)),component=int(floor(gl_FragCoord.y));
  if(i==1999){
   vec2 e=calculatedFanError();gl_FragColor=packFloat(component==0?e.x:(component==1?e.y:0.0));return;
  }
  if(i>=gridD()+2*gridN()){gl_FragColor=vec4(0.0);return;}
  vec4 s=calculatedSegment(i);
  gl_FragColor=packFloat(component==0?s.x:(component==1?s.y:(component==2?s.z:s.w)));return;
 }
 float fit=min(RENDERSIZE.x/960.0,RENDERSIZE.y/640.0)*viewZoom;
 vec2 p=(vec2(gl_FragCoord.x,RENDERSIZE.y-gl_FragCoord.y)-RENDERSIZE*.5)/fit+vec2(480.0,320.0);
 float c=agGeometryCoverage(p,.55/fit);gl_FragColor=c>=1.0/255.0?vec4(vec3(1.0),c):vec4(0.0);
}

void main(){if(PASSINDEX==2){gl_FragColor=agCalculateBounds();return;}agOriginalMain();}
