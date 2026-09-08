extends RefCounted
const Bindings = preload("res://scripts/input_bindings.gd")

const Copy = preload("res://scripts/copy.gd")
const STRINGS := {
	"edition": ["THE SHIPPING SHRINE", "배송 신전"],
	"menu_intro": ["The gods demand next-day delivery.\nYour lunch break has been sacrificed.", "신께서 익일 배송을 원하십니다.\n당신의 점심시간은 제물로 바쳤습니다."],
	"campaign": ["START SOLO CAMPAIGN", "혼자 캠페인 시작"],
	"host_campaign": ["HOST CO-OP CAMPAIGN", "협동 캠페인 방 만들기"],
	"practice_short": ["SHORT PRACTICE", "짧은 연습"],
	"help": ["{help}  SHIFT MANUAL", "{help}  근무 안내서"],
	"help_title": ["SHIFT MANUAL", "근무 안내서"],
	"help_body": ["STANDARD — stable cargo\nSNEEZER — blasts cargo forward\nCLINGER — sticks on contact\nHOPPER — jumps when left alone\n\n{forward}/{left}/{back}/{right} move  ·  MOUSE look  ·  {jump} jump  ·  ESC menu\n{interact} pick up / catch  ·  {throw} throw  ·  {lever} reverse belt\nRelay +5: a coworker catches your airborne throw 3 m away\nwithin 3 seconds, then delivers it to the correct bay.\n{ping} ping  ·  {help} close help\n{horn} airhorn scares a nearby Packrat.", "일반 상자 — 안정적인 화물\n재채기 상자 — 앞쪽 화물을 밀어냄\n접착 상자 — 닿으면 달라붙음\n점프 상자 — 내버려 두면 뛰어오름\n\n{forward}/{left}/{back}/{right} 이동  ·  마우스 시점  ·  {jump} 점프  ·  ESC 메뉴\n{interact} 집기 / 받기  ·  {throw} 던지기  ·  {lever} 벨트 전환\n릴레이 +5: 동료가 3m 밖에서 3초 안에 공중으로 받고\n올바른 출고구까지 배송해야 합니다.\n{ping} 위치 알림  ·  {help} 안내서 닫기\n{horn} 경적으로 가까운 포장쥐를 쫓아내세요."],
	"help_close": ["CLOSE MANUAL", "안내서 닫기"],
	"contract": ["CONTRACT %d / 3", "계약 %d / 3"],
	"bank": ["BANK  %d CREDITS", "공동 금고  %d 크레딧"],
	"run_progress": ["%d deliveries  ·  %d relays", "배송 %d개  ·  릴레이 %d회"],
	"earned": ["THIS CONTRACT  +%d", "이번 계약  +%d"],
	"record": ["COMPLETED RUNS  %d", "완주 기록  %d회"],
	"contract_clear": ["CONTRACT COMPLETE", "계약 완료"],
	"contract_failed": ["CONTRACT FAILED / BANK UNCHANGED", "계약 실패 / 금고 변동 없음"],
	"run_clear": ["ALL CONTRACTS COMPLETE", "모든 계약 완료"],
	"buy_boots": ["BOOTS  20C  ·  +8%% SPEED  [%d/2]", "작업화  20C  ·  속도 +8%%  [%d/2]"],
	"buy_time": ["PERMIT  25C  ·  +20 SEC  [%d/2]", "연장 허가  25C  ·  +20초  [%d/2]"],
	"buy_horn": ["AIRHORN KIT  20C  ·  6 SEC  [%d/1]", "경적 키트  20C  ·  6초  [%d/1]"],
	"next_contract": ["READY FOR NEXT CONTRACT  %d/%d", "다음 계약 준비  %d/%d"],
	"ready_wait": ["READY / WAITING FOR CREW", "준비 완료 / 동료 대기 중"],
	"settings": ["DISPLAY & SOUND", "화면 및 소리"],
	"volume": ["MASTER VOLUME  %d%%", "전체 음량  %d%%"],
	"fov": ["FIELD OF VIEW  %d°", "시야각  %d°"],
	"fullscreen_on": ["FULLSCREEN  ON", "전체 화면  켜짐"],
	"fullscreen_off": ["FULLSCREEN  OFF", "전체 화면  꺼짐"],
	"progress": ["%s", "%s"],
	"horn_ready": ["{horn}  AIRHORN READY", "{horn}  경적 준비"],
	"horn_wait": ["AIRHORN  %.1fs", "경적  %.1f초"],
	"packrat_off": ["", ""],
	"packrat_patrol": ["PACKRAT / PATROLLING", "포장쥐 / 순찰 중"],
	"packrat_seek": ["PACKRAT / APPROACHING CARGO!", "포장쥐 / 상자에 접근 중!"],
	"packrat_warning": ["PACKRAT / STEALING!", "포장쥐 / 훔치려는 중!"],
	"packrat_carry": ["PACKRAT / CARRYING CARGO", "포장쥐 / 화물 운반 중"],
	"packrat_flee": ["PACKRAT / FLEEING", "포장쥐 / 도망치는 중"],
	"quit": ["QUIT GAME", "게임 종료"]
}

static func get_text(key: String) -> String:
	var pair: Array = STRINGS.get(key, [key, key])
	return Bindings.expand(pair[1 if Copy.language == "ko" else 0])
