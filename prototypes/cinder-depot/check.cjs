const fs=require('fs'),vm=require('vm'),assert=require('assert');
const html=fs.readFileSync(require('path').join(__dirname,'index.html'),'utf8');let js=html.match(/<script>([\s\S]*?)<\/script>/)[1];
const els={};const el=id=>els[id]||(els[id]={checked:id==='routes',style:{},focus(){},getContext(){return {}},addEventListener(){}});
const context={document:{getElementById:el,documentElement:{},body:{classList:{contains:()=>false}},addEventListener(){}},window:{addEventListener(){}},requestAnimationFrame(){},console};
js=js.replace('reset();requestAnimationFrame(frame);',`reset();window.test={route,point,grid,ship,receiver,clue,rooms,can,interact,drop,update,reset,go(p){path=route(player,p);for(let i=0;i<20000&&path.length;i++)update(.04);return dist(player,p)},get:()=>({player,state,won,emergency}), gateTest(){player={x:230,y:520};interact();if(emergency)throw Error('outside unlock');player={x:280,y:520};interact();return emergency}};`);
vm.runInNewContext(js,context);const t=context.window.test;

for(let i=0;i<t.grid.length;i++)if(t.grid[i]&&t.can(t.point(i)))assert(t.route(t.ship,t.point(i)).length||Math.hypot(t.point(i).x-t.ship.x,t.point(i).y-t.ship.y)<30,'unreachable '+i);
t.interact();assert.equal(t.get().state,'carry');assert(t.go(t.receiver)<20);t.interact();assert.equal(t.get().state,'receipt');t.interact();assert.equal(t.get().state,'return');assert(t.go(t.ship)<20);t.interact();assert(t.get().won);t.reset();assert(t.gateTest());t.reset();t.interact();t.drop();assert.equal(t.get().state,'pickup');t.interact();assert.equal(t.get().state,'carry');console.log('PASS: closed-gate connectivity, full movement/delivery/receipt/return, gate side restriction, pickup/drop.');
