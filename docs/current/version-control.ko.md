# Git 버전 관리

[en](version-control.en.md)

현재 0.8.14 Unity 프로젝트·아트 소스·도구·한영 문서를 Git으로 보존한다. 기존 Godot와 임시 프로젝트 삭제도 이력에 기록하며 과거 커밋은 보존한다. 완료한 작업은 관련 검사·문서 갱신 후 커밋하고 origin에 일반 푸시한다. 빌드·artifacts 검증 출력·Unity 캐시·개인 경로·인증정보·Blender 자동 백업은 제외한다. 소스에서 Unity 빌드와 검사 도구를 다시 실행할 수 있다. 원격은 GitHub 비공개 저장소이며 파일 접근 권한 변경과 공개 전환은 별도 요청이 필요하다. 현재 기준점 커밋은 여러 미커밋 작업을 묶은 실제 스냅샷이며, 과거 개별 작업 커밋을 소급해 꾸며 만들지 않는다.

GitHub 원격: https://github.com/LeeDoik/no-returns (비공개). 모델·Blender 원본·텍스처·음원·영상·압축 에셋은 .gitattributes에 따라 Git LFS로 저장한다. 복제 환경은 Git LFS 설치 후 git lfs pull을 실행한다. 과거 커밋의 일반 Git 바이너리는 이력 보존을 위해 재작성하지 않았다. 과거 텍스트 객체 783개와 현재 대상 파일의 자격증명 패턴 검사에서 발견 0개.
