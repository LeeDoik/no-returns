using System;
using System.IO;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
namespace NoReturns.CarryLab {
[Serializable] public sealed class CarryProgress {
    public int version,credits,deliveries;
    public bool beacon;
    public string checksum;
}
// Local host progression only. Never deserialize live mission/physics state.
public sealed class CarrySave {
    readonly string path;
    string lastChecksum;
    bool preserveBackup;
    public CarrySave(string path){this.path=path;}
    static string Checksum(CarryProgress p){
        string raw=p.version.ToString(CultureInfo.InvariantCulture)+"|"+p.credits.ToString(CultureInfo.InvariantCulture)+"|"+p.deliveries.ToString(CultureInfo.InvariantCulture)+"|"+(p.beacon?"1":"0");
        using(var hash=SHA256.Create())return Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes(raw)));
    }
    static CarryProgress Read(string file){
        if(new FileInfo(file).Length>4096)throw new InvalidDataException("Save too large");
        var p=JsonUtility.FromJson<CarryProgress>(File.ReadAllText(file,Encoding.UTF8));
        if(p!=null&&p.version>1)throw new NotSupportedException("Newer save version");
        if(p==null||p.version!=1||p.credits<0||p.deliveries<0||p.checksum!=Checksum(p))throw new InvalidDataException("Invalid save");
        return p;
    }
    public bool Load(out CarryProgress progress,out string notice){
        progress=null;notice="New host progress";
        if(!File.Exists(path)&&!File.Exists(path+".bak"))return true;
        try{progress=Read(path);lastChecksum=progress.checksum;notice="Host progress loaded";return true;}catch(NotSupportedException){notice="Newer save version; update the game. Files preserved.";return false;}catch(Exception e)when(e is InvalidDataException||e is IOException||e is UnauthorizedAccessException||e is ArgumentException){}
        try{progress=Read(path+".bak");preserveBackup=true;notice="Recovered previous backup; recent changes may be missing";return true;}catch(NotSupportedException){notice="Newer save version; update the game. Files preserved.";return false;}catch(Exception e)when(e is InvalidDataException||e is IOException||e is UnauthorizedAccessException||e is ArgumentException){}
        notice="Cannot read host save. Original files preserved.";return false;
    }
    public bool Write(CarryMission mission,out string notice){
        notice=null;
        var p=new CarryProgress{version=1,credits=mission.Credits,deliveries=mission.SuccessfulDeliveries,beacon=mission.BeaconUnlocked};p.checksum=Checksum(p);
        if(p.checksum==lastChecksum)return true;
        try{
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            byte[] bytes=Encoding.UTF8.GetBytes(JsonUtility.ToJson(p,true));
            using(var file=new FileStream(path+".tmp",FileMode.Create,FileAccess.Write,FileShare.None)){file.Write(bytes,0,bytes.Length);file.Flush(true);}
            if(File.Exists(path))File.Replace(path+".tmp",path,preserveBackup?null:path+".bak");
            else File.Move(path+".tmp",path);
            lastChecksum=p.checksum;preserveBackup=false;notice="Progress saved on host PC";return true;
        }catch(Exception e)when(e is InvalidDataException||e is IOException||e is UnauthorizedAccessException||e is ArgumentException){notice="SAVE FAILED / progress is not on disk; retrying";return false;}
    }
}
}
