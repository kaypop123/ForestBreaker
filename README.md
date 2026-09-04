#  ForestBreaker

> 다수의 적을 상대하며 근접 공격과 다양한 스킬을 활용해 전투를 진행하는  
> **2D 횡스크롤 액션 게임**

**개발 형태:** 1인 개발  
**플랫폼:** PC (Windows)  
**장르:** 2D 횡스크롤 액션  
**개발 엔진:** Unity 6  
**개발 언어:** C#  
**개발 기간:** 2026.03 ~ 2026.04  

---

## 🎮 프로젝트 개요

ForestBreaker는 다수의 적이 등장하는 전투 상황에서  
플레이어가 근접 공격과 다양한 스킬을 활용하여 스테이지를 진행하는 2D 횡스크롤 액션 게임입니다.

1인 개발 프로젝트로 플레이어 전투부터 적 생성, 아이템, 스테이지 진행 시스템까지 직접 설계 및 구현했습니다.

특히 반복적인 Enemy 생성/삭제 과정에서 발생하는 비용을 개선하기 위해  
**Prefab별 Object Pooling 시스템을 구현하고 Unity Profiler를 통해 적용 전후 성능을 직접 측정했습니다.**

---
## 🎥 게임 플레이 영상

> ForestBreaker의 실제 게임 플레이 영상입니다.

https://github.com/user-attachments/assets/(https://drive.google.com/file/d/1_geBa20EXmybbaAaOl_o6gjpc58Lhkc6/view?usp=sharing)

---

## 🎮 게임 장면

<table width="100%">
  <tr>
    <td width="50%">
      <img src="이미지주소1" width="100%">
    </td>
    <td width="50%">
      <img src="이미지주소2" width="100%">
    </td>
  </tr>
</table>

---

## 📈 Object Pooling 적용 결과

| 측정 항목 | 기존 방식 | Object Pooling | 결과 |
|---|---:|---:|---:|
| Enemy Spawn | 5개 | 5개 | 동일 조건 |
| Instantiate | 5회 | 0회 | 런타임 추가 생성 제거 |
| GC Alloc | 11.6 KB | 80 B | 약 99.3% 감소 |
| 처리 시간 | 3.10 ms | 0.47 ms | 약 84.8% 감소 |

**측정 환경**
- Unity 6
- Editor Play Mode
- Intel Core i5-1340P
- RAM 16GB
- Windows 11

---

## 🔎 주요 구현 코드

제가 직접 설계 및 구현한 코드 중 핵심 시스템을 확인할 수 있습니다.

### 👾 Enemy Spawner & Object Pooling

📄 [EnemySpawner.cs](https://github.com/kaypop123/ForestBreaker/blob/main/Assets/Scripts/EnemyCS/EnemySpawner.cs)

- Enemy Prefab별 독립적인 Object Pool 관리
- 비활성화된 Enemy 객체를 우선 탐색하여 재사용
- 사용 가능한 객체가 없을 경우에만 새로운 객체 생성
- Pool 부족 시 자동으로 확장되는 동적 Object Pooling 구조
- Enemy 사망 후 Pool로 반환하여 재사용

---

### ⚔️ 플레이어 전투 시스템

📄 [PlayerCombatController.cs](https://github.com/kaypop123/ForestBreaker/blob/main/Assets/Scripts/PlayerCS/PlayerCombatController.cs)

📄 [PlayerAttackTrigger.cs](https://github.com/kaypop123/ForestBreaker/blob/main/Assets/Scripts/PlayerCS/PlayerAttackTrigger.cs)

- 플레이어 전투 상태 및 공격 처리
- 공격 판정과 실제 전투 로직의 역할 분리
- Enemy 피격 시스템과 연계

---

### 🏃 플레이어 컨트롤

📄 [PlayerController.cs](https://github.com/kaypop123/ForestBreaker/blob/main/Assets/Scripts/PlayerCS/PlayerController.cs)

📄 [PlayerInputHandler.cs](https://github.com/kaypop123/ForestBreaker/blob/main/Assets/Scripts/PlayerCS/PlayerInputHandler.cs)

- 플레이어 이동 및 행동 제어
- 입력 처리와 캐릭터 행동 로직 분리
- 플레이어 상태에 따른 행동 제어

---

### 💨 대시 시스템

📄 [PlayerDashController.cs](https://github.com/kaypop123/ForestBreaker/blob/main/Assets/Scripts/PlayerCS/PlayerDashController.cs)

- 플레이어 대시 행동 제어
- 전투 및 이동 상태와 연계한 대시 처리

---

### ⚡ 플레이어 스킬 시스템

📄 [PlayerSkillController.cs](https://github.com/kaypop123/ForestBreaker/blob/main/Assets/Scripts/PlayerCS/PlayerSkillController.cs)

📄 [SlashWave.cs](https://github.com/kaypop123/ForestBreaker/blob/main/Assets/Scripts/PlayerCS/SlashWave.cs)

- 플레이어 스킬 사용 및 상태 관리
- SlashWave 스킬 동작 구현
- 전투 시스템과 스킬 로직 연계

---

### 🛡️ 방어 시스템

📄 [PlayerBlockingController.cs](https://github.com/kaypop123/ForestBreaker/blob/main/Assets/Scripts/PlayerCS/PlayerBlockingController.cs)

- 플레이어 방어 상태 관리
- 전투 상태와 연계한 방어 행동 처리

---

### 🔄 플레이어 상태 관리

📄 [PlayerState.cs](https://github.com/kaypop123/ForestBreaker/blob/main/Assets/Scripts/PlayerCS/PlayerState.cs)

- 플레이어 행동 상태 관리
- 이동 / 공격 / 스킬 등 플레이어 시스템 간 상태 공유

---

### 🗺️ Enemy 및 스테이지 진행

📄 [EnemyState.cs](https://github.com/kaypop123/ForestBreaker/blob/main/Assets/Scripts/EnemyCS/EnemyState.cs)

📄 [StageFlowManager.cs](https://github.com/kaypop123/ForestBreaker/blob/main/Assets/Scripts/EnemyCS/StageFlowManager.cs)

📄 [StageManager.cs](https://github.com/kaypop123/ForestBreaker/blob/main/Assets/Scripts/StageManager.cs)

- Enemy 상태 및 사망 처리
- Enemy 사망 이벤트 기반 스테이지 진행
- 스테이지별 Enemy Spawn 흐름 관리

---

### 👹 보스 시스템

📄 [BossCore.cs](https://github.com/kaypop123/ForestBreaker/blob/main/Assets/Scripts/EnemyCS/BossCore.cs)

📄 [BossAttack.cs](https://github.com/kaypop123/ForestBreaker/blob/main/Assets/Scripts/EnemyCS/BossAttack.cs)

📄 [BossSkill.cs](https://github.com/kaypop123/ForestBreaker/blob/main/Assets/Scripts/EnemyCS/BossSkill.cs)

- 보스 핵심 행동 로직
- 공격 및 스킬 기능 분리
- 보스 전투 시스템 구성

---

# 🚀 Object Pooling 성능 개선

## Before

기존 구조에서는 웨이브가 시작될 때마다 필요한 Enemy를 `Instantiate()`로 생성하고,  
Enemy가 제거될 때 `Destroy()`를 호출했습니다.

스테이지가 진행될수록 등장하는 Enemy의 수가 증가하기 때문에  
반복적인 생성과 제거에 따른 CPU 처리 비용과 GC Alloc이 발생했습니다.

```text
Enemy 5개 생성

Instantiate : 5회
GC Alloc    : 11.6 KB
처리 시간   : 3.10 ms
