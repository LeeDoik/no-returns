export default async ({ project }) => {
 const base=process.env.NR_TRAILER_MEDIA || "/home/user/nr-trailer";
 const p=await project({dir:"project",size:"1280x720",fps:24,background:"#080b0d"});
 const still=async(file,at,dur,name)=>{const h=await p.add(base+"/"+file.replace(".png","-pan.mp4"));p.cut(h,{from:0,dur,at,fit:"cover"});};
 const title=(at,dur,small,big,sub)=>p.compose(
 <frame width={1280} height={720} layout="none" background="#111817">
 <rect x={104} y={170} width={72} height={6} fill="#d39b5e"/>
 <text x={104} y={196} width={1070} height={45} fontFamily="Montserrat" fontSize={21} letterSpacing={4} color="#aaaf9f">{small}</text>
 <text x={98} y={259} width={1100} height={130} fontFamily="Anton" fontSize={105} color="#e6dfc9">{big}</text>
 <text x={104} y={416} width={1060} height={70} fontFamily="Montserrat" fontSize={27} color="#cbbd9f">{sub}</text>
 <rect x={104} y={525} width={1070} height={2} fill="#45504b"/>
 </frame>,{at,dur,name:big});
 title(0,3,"PERSONNEL DIVISION / CINDER OPERATIONS","NO RETURNS","A place for every parcel. A purpose for every employee.");
 await still("exterior.png",3,5,"A scattered home");
 title(8,5,"INTERPLANETARY ESSENTIAL SUPPLY","STILL CONNECTED","AIR FILTERS   /   MEDICINE   /   FOOD");
 await still("arrival.png",13,5,"Arrival");
 p.cut(await p.add(base+"/depot-motion.mp4"),{from:0,dur:6,at:18,fit:"cover"});
 await still("carry.png",24,4,"Carry");
 await still("stable.png",28,5,"Protected delivery zone");
 p.cut(await p.add(base+"/threat-clean.mp4"),{from:0,dur:6,at:33,fit:"cover"});
 await still("wait.png",39,6,"Never alone");
 await still("assist.png",45,3,"Assist");
 await still("extract.png",48,3,"Extract");
 await still("receipt.png",51,7,"The familiar sound");
 await still("clue.png",58,4,"Someone was here");
 await still("aboard.png",62,5,"Someone is counting on us");
 p.compose(<frame width={1280} height={720} background="#030504"/>,{at:67,dur:2,name:"Signal lost"});
 title(69,6,"1–4 PLAYER COOPERATIVE DELIVERY HORROR","NO RETURNS","Delivery guaranteed. Return not included.");
 p.compose(<frame width={1280} height={720} layout="none">
 <rect x={0} y={0} width={1280} height={48} fill="#080b0d"/>
 <rect x={0} y={638} width={1280} height={82} fill="#080b0d"/>
 <text x={30} y={15} width={1220} height={26} fontFamily="Montserrat" fontSize={13} letterSpacing={2} color="#9ca696">NO RETURNS / CONCEPT TRAILER 01                                      NOT GAMEPLAY</text>
 </frame>,{at:0,dur:75,name:"Concept disclosure and subtitle safe area"});
 await p.frame(20,base+"/review.png");
 await p.render(base+"/picture.mp4",{depth:8,accel:"cpu",concurrency:2});
};
