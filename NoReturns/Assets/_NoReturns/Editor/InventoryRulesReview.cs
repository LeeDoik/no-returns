using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using NoReturns.CarryLab;
namespace NoReturns.EditorTools {
public static class InventoryRulesReview {
 [Serializable] public class Result {public string status="PASS";public List<string> checks=new List<string>();}
 public static object Run(){var r=new Result();void Check(bool ok,string name){if(!ok)throw new Exception(name);r.checks.Add(name);}
 var m=new CarryMission(new CarryProgress{credits=400},true);var inv=new CarryInventory();
 Check(!inv.Select(0,1,1)&&inv.Select(0,1,2),"co-op one slot and solo two");
 Check(!inv.Buy(0,2,m,false)&&m.Credits==400,"outside purchase rejected");
 Check(inv.Buy(0,2,m,true)&&m.Credits==360,"buy and equip consumes 40 wallet funds");Check(!inv.Buy(0,2,m,true)&&m.Credits==360,"occupied slot cannot buy twice");
 inv.Select(0,0,2);Check(inv.Buy(0,2,m,true)&&m.Credits==320,"solo duplicate medicine permitted");
 m.Act(true,true);m.Act(true,true);m.BeginVisit(3);var v=m.Visit;Check(!inv.Buy(1,1,m,true)&&m.Credits==320,"field cannot restock");
 Check(!inv.Use(0,1,1,v,true)&&inv.Medicine[0]==1,"healthy target consumes nothing");v.Damage(1,100);Check(!inv.Use(0,1,1,v,true),"downed target cannot be healed");v.Rescue(1);
 Check(!inv.Use(0,1,1,v,false)&&inv.Medicine[0]==1,"blocked target consumes nothing");Check(inv.Use(0,1,1,v,true)&&v.Health[1]==100&&v.Downs[1]==1,"healing restores health without clearing down count");
 inv.Medicine[2]=1;Check(!inv.Use(1,1,1,v,true)&&inv.Medicine[2]==1,"second simultaneous heal consumes nothing");
 Check(!inv.Drop(0,1,Vector3.zero),"empty slot cannot duplicate ground medicine");Check(inv.Drop(1,1,Vector3.one)&&inv.Medicine[2]==0&&inv.Ground.Count==1,"drop moves one dose to world");
 int id=inv.Ground[0].id;Check(inv.PickUp(0,id,1)&&!inv.PickUp(1,id,1),"one pickup wins and cannot duplicate");Check(inv.Medicine[1]==1,"co-op hides but retains prior solo second slot");
 v.Damage(0,60);Check(inv.Use(0,0,1,v,true)&&v.Health[0]==100,"self healing");
 v.Eliminate(0);Check(!inv.Use(0,1,2,v,true)&&inv.Medicine[1]==1,"elimination retains unused dose but blocks use");v.Fail();Check(inv.Medicine[1]==1,"visit failure preserves carried medicine");
 Check(!inv.Select(-1,0,1)&&!inv.Select(0,9,1)&&!inv.Buy(8,1,m,true),"invalid actor and slot rejected");
 var snapshot=inv.Snapshot(1);snapshot.medicine[1]=9;Check(inv.Medicine[1]==1&&!snapshot.Ready,"snapshot copy and validation");
 var poor=new CarryMission(new CarryProgress{credits=39},true);Check(!inv.Buy(2,1,poor,true)&&poor.Credits==39,"insufficient funds untouched");
 string path=Path.GetFullPath("../artifacts/inventory-rules.json");Directory.CreateDirectory(Path.GetDirectoryName(path));File.WriteAllText(path,JsonUtility.ToJson(r,true));return r;
 }
}
}
