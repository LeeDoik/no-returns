using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace NoReturns.CarryLab {
// Bounded newline frames. Main-thread polling keeps Unity state off network threads.
public sealed class CarryWire : IDisposable {
    public TcpClient socket;
    readonly List<byte> pending = new List<byte>();
    public bool Closed { get; private set; }
    public CarryWire(TcpClient client) { socket=client; socket.NoDelay=true; socket.SendTimeout=200; }
    public List<string> Read() {
        var result=new List<string>();
        if(Closed)return result;
        try {
            if(socket.Client.Poll(0,SelectMode.SelectRead) && socket.Available==0){Dispose();return result;}
            int budget=65536;
            while(socket.Available>0 && budget>0) {
                byte[] bytes=new byte[Math.Min(Math.Min(socket.Available,4096),budget)];
                int n=socket.GetStream().Read(bytes,0,bytes.Length);budget-=n;
                for(int i=0;i<n;i++) {
                    if(bytes[i]==10){ result.Add(Encoding.UTF8.GetString(pending.ToArray()));pending.Clear(); }
                    else pending.Add(bytes[i]);
                    if(pending.Count>32768 || result.Count>128)throw new Exception("Frame budget exceeded");
                }
            }
        }catch{Dispose();}
        return result;
    }
    public void Send(string value){if(Closed)return;try{var b=Encoding.UTF8.GetBytes(value+"\n");socket.GetStream().Write(b,0,b.Length);}catch{Dispose();}}
    public void Dispose(){Closed=true;try{socket.Close();}catch{}}
}
[Serializable] public class CarryInput { public int seq; public float x,z,yaw,pitch; public bool jump,interact,drop,reset,action,quiet,call,shove,rescue,buy,contract,deploy,inspect; }
[Serializable] public class CarryState { public int protocol=9,clueMask,phase=-1,credits,receipt,returnPay,tick,holder=-1,ack,players; public UnityEngine.Vector3 p0,p1,cargo; public UnityEngine.Quaternion rotation; public int beaconCarrier=-1; public bool beaconExists; public bool receiptCollected,receiptReady; public float receiptProgress; public float yaw0,yaw1,shiftElapsed; public int suppressionStage; public ThreatState outerDanger; public bool unlocked,hard; public int deliveries,charges; public float beaconTime; public UnityEngine.Vector3 beaconPosition; public bool hazard; public ThreatState danger; public bool connected; public string message,rejection; }
}
