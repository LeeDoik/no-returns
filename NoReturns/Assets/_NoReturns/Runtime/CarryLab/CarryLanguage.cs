using System;
using System.Collections.Generic;
using UnityEngine;
namespace NoReturns.CarryLab {
// English remains the source copy. Each process renders its own chosen language.
public static class CarryLanguage {
    public static bool Korean {get;private set;}=true;
    static bool persist;
    static string preference="NoReturns.Language";
    public static Font Font {get;private set;}
    public static void Initialize(string testFolder){
        persist=testFolder==null;
        preference=testFolder==null?"NoReturns.Language":"NoReturns.Language.Test."+testFolder;
        Korean=!persist||PlayerPrefs.GetString(preference,"ko")!="en";
        Font=UnityEngine.Font.CreateDynamicFontFromOSFont(new[]{"Malgun Gothic","맑은 고딕","Arial"},20);
    }
    public static void Toggle(){Korean=!Korean;if(!persist)return;try{PlayerPrefs.SetString(preference,Korean?"ko":"en");PlayerPrefs.Save();}catch(PlayerPrefsException){Debug.LogWarning("Language changed for this session; preference could not be saved.");}}
    public static string Text(string english){
        if(!Korean)return english;
        if(Ko.TryGetValue(english,out var value))return value;
        if(english.StartsWith("Host failed: ",StringComparison.Ordinal))return "방을 만들지 못했습니다. 이미 열린 방이나 네트워크 설정을 확인하세요.";
        if(english.StartsWith("Join failed: ",StringComparison.Ordinal))return "참가하지 못했습니다. 주소와 방 상태를 확인하세요.";
        return english;
    }
    public static readonly Dictionary<string,string> Ko=new Dictionary<string,string>{
        {"Protocol mismatch / use the same game build","게임 버전이 다릅니다. 같은 실행본으로 접속하세요."},
        {"RECEIPT COLLECTED","영수증 회수 완료"},{"TAKE RECEIPT [E]","영수증 가져가기 [E]"},{"PRINTING RECEIPT","영수증 출력 중"},{"SCANNING","화물 확인 중"},{"PLACE PARCEL","화물을 놓으세요"},{"STANDBY","대기 중"},
        {"Receipt collected / return aboard to get paid","영수증을 챙겼습니다 / 우주선에 돌아가 정산하세요"},
        {"Collect receipt at terminal [E] / No pay until return","단말기에서 [E]로 영수증을 챙기세요 / 귀환 전에는 미지급"},
        {"COLLECT RECEIPT [E] at terminal / Bring it back aboard, then [E] RETURN TO GET PAID","단말기에서 [E] 영수증 회수 / 우주선으로 가져와 [E] 귀환·정산"},
        {"PLACE PARCEL ON MARKED FLOOR","표시 안에 화물을 내려놓으세요"},
        {"SCANNING / KEEP CLEAR","확인 중 / 화물을 움직이지 마세요"},
        {"DELIVERY CONFIRMED","배송 완료"},
        {"TERMINAL STANDBY","단말 대기"},
        {"SUPPRESSOR / steady hum","억제장치 / 일정한 작동음"},
        {"SUPPRESSOR / irregular signal - plan your return","억제장치 / 불규칙한 신호 — 귀환을 준비하세요"},
        {"SUPPRESSOR / failing - movement at the east gate","억제장치 / 붕괴 임박 — 동쪽 문에서 무언가 움직입니다"},
        {"SUPPRESSOR OFF / outer creature entering - return to ship","억제장치 정지 / 외곽 생물 진입 — 우주선으로 대피하세요"},
        {"Partner left / emergency recovery / secured pay retained","동료가 나갔습니다 / 긴급 회수 완료 / 확보한 보수 유지"},
        {"Shift in progress / wait for host to prepare next shift","근무 진행 중 / 방장이 다음 근무를 준비하면 참가할 수 있습니다"},
        {"Room full / wait for a free crew slot","방이 가득 찼습니다 / 직원 자리가 생기면 다시 참가하세요"},
        {CarryClues.FirstTitle,"01 / 끝난 근무"},
        {CarryClues.FirstBody,"정비 기록: 이 배송소는 열아홉 근무 전에 폐쇄됐다.\n단말은 따뜻하다. 급지 롤러에는 마르지 않은 잉크가 묻어 있다.\n누군가 계속 사용하고 있다."},
        {CarryClues.SecondTitle,"02 / 이미 수령함"},
        {CarryClues.SecondBody,"화물 CND-041: 수령 완료.\n영수증의 날짜는 우리 우주선이 착륙하기 전이다.\n수령인 서명: 직원 00.\n셔터 너머에서는 아무도 대답하지 않는다."},
        {"Inspect clues during the delivery shift","현장에 도착한 뒤 단서를 조사하세요"},
        {"Aim at a nearby terminal and press E","가까운 단말을 바라보고 E로 조사하세요"},
        {"Recorder idle / deliver the parcel first","수령 기록기 대기 중 / 먼저 화물을 배송하세요"},
        {"Already in the shared field log","이미 공용 현장 기록에 있습니다"},
        {"Clue shared / Tab to read safely aboard","단서를 공유했습니다 / 우주선에서 Tab으로 안전하게 읽으세요"},
        {"Shift quiet walk / C call / LMB baton / Hold E rescue / E inspect / Tab field log","Shift 조용히 걷기 / C 소리 / 왼쪽 클릭 충격봉 / E 유지 구조 / E 조사 / Tab 기록"},
        {"[E] Inspect terminal / shared field log","[E] 단말 조사 / 공용 현장 기록"},
        {"CINDER DEPOT / SHARED FIELD LOG","신더 배송소 / 공용 현장 기록"},
        {"The world keeps moving. Read aboard. Records reset on next arrival; not saved after exit.","읽는 동안에도 현장은 움직입니다. 우주선에서 읽으세요. 기록은 다음 도착 때 초기화되며 종료 후 저장되지 않습니다."},
        {"UNRECORDED / inspect the site","미발견 / 현장을 조사하세요"},
        {"No entry yet. A teammate can share it by inspecting a terminal.","아직 기록이 없습니다. 동료가 단말을 조사해도 함께 공유됩니다."},
        {"Not discovered","미발견"},
        {"CLOSE / Tab or Esc","닫기 / Tab 또는 Esc"},
        {"SHIP SUPPLY / saved host license","우주선 보급 / 방장 저장 사용권"},
        {"New host progress","새 방장 진행을 시작합니다"},
        {"Host progress loaded","방장의 저장된 진행을 불러왔습니다"},
        {"Progress saved on host PC","방장 PC에 진행 저장 완료"},
        {"Recovered previous backup; recent changes may be missing","이전 백업으로 복구했습니다. 최근 변경 일부가 없을 수 있습니다"},
        {"Cannot read host save. Original files preserved.","저장 파일을 읽을 수 없어 방을 열지 못했습니다. 원본 파일은 보존했습니다"},
        {"Newer save version; update the game. Files preserved.","더 최신 버전의 저장입니다. 게임을 업데이트하세요. 파일은 보존했습니다"},
        {"SAVE FAILED / progress is not on disk; retrying","저장 실패 / 진행이 파일에 반영되지 않았습니다. 다시 시도 중입니다"},
        {"Using host progress / personal save unchanged","방장 진행을 함께 사용 중 / 내 저장에는 반영하지 않습니다"},
        {"[E] ship action / Blue floor: ship / Gold floor: reception / Host save","[E] 우주선 조작 / 파란 바닥: 우주선 / 금색 바닥 표시: 수령소 / 방장 저장"},
        {"Beacon license purchased","유인 신호기 사용권을 구매했습니다"},
        {"Purchase unavailable: host, ship/report, 120 CR required","구매 불가: 방장이 우주선 준비·정산 중 120 CR로 구매할 수 있습니다"},
        {"Contract changed","계약 위험도를 변경했습니다"},
        {"Select route aboard after first delivery; host only","첫 배송 성공 후 우주선에서 항로 선택 중 방장이 변경할 수 있습니다"},
        {"Beacon deployed","유인 신호기를 설치했습니다"},
        {"Aim at yard floor within 8m, empty hands; beacon must be ready","빈손으로 8m 안 작업장 바닥을 조준하세요. 사용 횟수와 활성 상태를 확인하세요"},
        {"SHIP SUPPLY / session license","우주선 보급 / 이번 세션 사용권"},
        {"Wallet {0} CR / Beacon {1}","공용 잔액 {0} CR / 신호기 {1}"},
        {"OWNED","구매 완료"},
        {"120 CR","120 CR"},
        {"Buy beacon license","신호기 사용권 구매"},
        {"Purchase spawns beacon aboard. E carry / Q place. Two uses per shift, 8 seconds each.","구매하면 우주선에 신호기가 생깁니다. E 운반 / Q 내려놓기. 근무마다 2회, 각 8초 작동. 위·아래 키로 메뉴 선택, E 확인."},
        {"Change selected contract","선택한 계약 위험도 변경"},
        {"RISK / receipt 450 + return 180","위험 / 배송 450 + 귀환 180"},
        {"STANDARD / receipt 300 + return 120","일반 / 배송 300 + 귀환 120"},
        {"{0} / Beacon {1}/2 ({2}s) / E carry / Q place","{0} / 신호기 {1}/2회 ({2}초) / E 들기 / Q 내려놓기"},
        {"RISK","위험 계약"},
        {"STANDARD","일반 계약"},
        {"Shift quiet walk / Q call / G shove (empty hands) / Hold R near teammate to rescue","Shift 조용히 걷기 / C 소리내기 / G 빈손 밀치기 / 동료 곁에서 E를 눌러 구조"},
        {"LISTENER: ATTACK WARNING - MOVE AWAY","리스너: 공격 예고! 뒤로 피하세요"},
        {"LISTENER: STUNNED","리스너: 밀쳐져 잠시 멈춤"},
        {"LISTENER: INVESTIGATING SOUND","리스너: 마지막 소음 위치를 조사 중"},
        {"LISTENER: PATROLLING / keep quiet","리스너: 순찰 중 / 조용히 이동하세요"},
        {"Rescue {0}% / Baton cooldown {1}s","구조 {0}% / 충격봉 재사용 {1}초"},
        {"DOWN / wait for teammate rescue. All down: emergency recovery.","행동 불능 / 동료의 구조를 기다리세요. 전원 다운 시 긴급 회수됩니다."},
        {"Employee down / parcel released","직원이 쓰러져 화물을 놓았습니다"},
        {"Emergency recovery / secured pay retained","긴급 회수 완료 / 확보한 배송 보수는 유지됩니다"},
        {"NO RETURNS / LISTENER TEST","NO RETURNS / 리스너 실험"},{"LISTENER TEST","리스너 실험"},{"TEAMMATE DOWN / put cargo down, approach and hold E","동료가 쓰러졌습니다 / 화물을 내려놓고 가까이서 E를 누르세요"},
        {"Choose HOST or enter the host's LAN address.","방을 만들거나 방장의 내부 네트워크 주소를 입력하세요."},
        {"HOST / waiting for partner","방장 / 동료를 기다리는 중"},
        {"Connecting...","연결 중..."},
        {"Disconnected. Host or join again.","연결이 종료됐습니다. 방을 만들거나 다시 참가하세요."},
        {"Connection failed. Check address / host / firewall.","연결에 실패했습니다. 주소·방장 접속·방화벽 설정을 확인하세요."},
        {"CLIENT / connected","참가자 / 연결됨"},
        {"Connection timed out.","연결 시간이 초과됐습니다."},
        {"HOST / partner connected","방장 / 동료가 참가했습니다"},
        {"HOST / partner left","방장 / 동료가 나갔습니다"},
        {"Host connection lost.","방장과 연결이 끊어졌습니다."},
        {"Host timed out.","방장의 응답이 없어 연결을 종료했습니다."},
        {"Parcel set down","화물을 내려놓았습니다"},
        {"Parcel held","화물을 들었습니다"},
        {"NO RETURNS / CARRY TEST","NO RETURNS / 운반 실험"},
        {"NO RETURNS / DELIVERY LOOP","NO RETURNS / 배송 실험"},
        {"HOST","방 만들기"},{"JOIN","참가하기"},{"RESUME","계속하기"},{"LEAVE SESSION","방 나가기"},{"QUIT","게임 종료"},
        {"Direct LAN / TCP 27841\nSame PC: 127.0.0.1\nAllow host through Windows Firewall if prompted.","내부 네트워크 직접 연결 / TCP 27841\n같은 PC에서 참가: 127.0.0.1\n방화벽 안내가 나오면 게임 연결을 허용하세요."},
        {"WASD move / Mouse look / E interact / Q set down\nSpace jump (empty hands) / R reset (host)","WASD 이동 / 마우스 시점 / E 상호작용 / Q 내려놓기\nSpace 빈손 점프 / R 실험실 초기화 (방장)"},
        {"WASD move / Mouse look / E interact / Q set down\nSpace jump (empty hands) / E ship action","WASD 이동 / 마우스 시점 / E 상호작용 / Q 내려놓기\nSpace 빈손 점프 / E 선내 항로·귀환"},
        {"FOV","시야각"},{"CARRY TEST","운반 실험"},{"DELIVERY LOOP","배송 실험"},{"Host","방장"},{"Client","참가자"},{"CREW","직원"},
        {"Move the parcel through the passage and onto the marked floor.","통로를 지나 화물을 선반 위에 내려놓으세요."},
        {"SHARED WALLET {0} CR / RECEIPT {1} / RETURN {2}","공용 잔액 {0} CR / 배송 보수 {1} / 귀환 보수 {2}"},
        {"[E] ship action / Blue floor: ship / Gold floor: reception / Session only","[E] 선내 행동 / 청록 바닥: 우주선 / 황색 바닥 표시: 수령소 / 종료하면 잔액 초기화"},
        {"[Q] SET DOWN   /   HANDS OCCUPIED","[Q] 내려놓기 / 화물을 들고 있습니다"},
        {"[E] PICK UP nearby parcel   /   WASD move   /   Esc menu","[E] 가까운 화물 들기 / WASD 이동 / Esc 메뉴"},
        {"SHIP / [E] Select CINDER DEPOT route","우주선 / [E] 신더 디포 항로 선택"},
        {"CINDER DEPOT selected / Crew aboard, then [E] AUTO ARRIVE","신더 디포 선택됨 / 전원 탑승 후 [E] 자동 이동·도착"},
        {"Deliver sealed parcel onto the marked reception floor. [E] aboard: abort and return","밀봉 화물을 황색 바닥 표시 안에 내려놓으세요. 선내 [E]: 배송을 포기하고 귀환"},
        {"RECEIPT CONFIRMED / Return to blue ship zone together, then [E] RETURN","수령 완료 / 청록색 선내 구역에 함께 모인 뒤 [E] 귀환"},
        {"SHIFT REPORT / [E] inside ship: prepare next shift","근무 정산 / 선내에서 [E] 다음 근무 준비"}
    };
}
}
