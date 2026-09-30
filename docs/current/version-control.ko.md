# Git 버전 관리

[en](version-control.en.md)

Unity 프로젝트·아트 소스·도구·한영 문서를 Git으로 보존한다. 기존 Godot와 임시 프로젝트 삭제도 이력에 기록하며 과거 커밋은 보존한다. 완료한 작업은 관련 검사·문서 갱신 후 커밋하고 origin에 일반 푸시하며 로컬 HEAD와 원격 브랜치 일치를 확인한다. 빌드·artifacts 검증 출력·Unity 캐시·개인 경로·인증정보·Blender 자동 백업은 제외한다. 소스에서 Unity 빌드와 검사 도구를 다시 실행할 수 있다. 2026-09-30 사용자 요청으로 공개 원격에서 개발하는 규칙으로 변경했다. 과거 비공개·로컬 커밋 전용 기록은 당시 상태로 보존한다. 기존 기준점 커밋은 여러 미커밋 작업을 묶은 실제 스냅샷이며, 과거 개별 작업 커밋을 소급해 꾸며 만들지 않는다.

GitHub 원격: [LeeDoik/no-returns](https://github.com/LeeDoik/no-returns) (공개). `gh repo view LeeDoik/no-returns --json isPrivate,visibility`로 PUBLIC·isPrivate=false를 확인했다. 이번에는 원격 공개 설정을 바꾸지 않았다. 모델·Blender 원본·텍스처·음원·영상·압축 에셋은 [.gitattributes](../../.gitattributes)에 따라 Git LFS로 저장하고 푸시 시 업로드를 확인한다. 복제 환경은 Git LFS 설치 후 git lfs pull을 실행한다. 과거 커밋의 일반 Git 바이너리는 이력 보존을 위해 재작성하지 않았다. 과거 자격증명 패턴 검사의 텍스트 객체 783개·발견 0개는 당시 검사 기록이며 이번 전체 이력 재검사 결과가 아니다. 현재 제외 규칙은 [.gitignore](../../.gitignore), 사용자별 Codex 설정 제외는 로컬 `.git/info/exclude`에서 관리한다.
