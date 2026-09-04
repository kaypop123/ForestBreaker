# 🌲 ForestBreaker

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

제가 직접 설계 및 구현한 코드 중 핵심 시스템입니다.

### 👾 Enemy Spawner & Object Pooling

📄 [EnemySpawner.cs](링크입력)

- Enemy Prefab별 독립적인 Object Pool 관리
- 비활성화된 Enemy 객체를 우선 탐색하여 재사용
- 사용 가능한 객체가 없을 경우에만 새로운 객체 생성
- Pool 부족 시 자동으로 확장되는 동적 Object Pooling 구조
- Enemy 사망 후 Destroy하지 않고 Pool로 반환

---

### ⚔️ 플레이어 전투 시스템

📄 [PlayerAttack.cs](링크입력)

- 근접 공격 처리
- 공격 범위 기반 Enemy 탐색
- `IDamageable` 인터페이스 기반 피격 처리
- 플레이어 공격과 Enemy 피격 시스템 간 의존성 감소

---

### 🏃 플레이어 이동

📄 [PlayerMovement.cs](링크입력)

- `Rigidbody2D` 기반 플레이어 이동
- 이동 및 방향 전환 처리
- 전투 상태와 연계한 캐릭터 행동 제어

---

### 🎁 아이템 시스템

📄 [Item System](링크입력)

- Enemy 사망 시 아이템 드롭
- 플레이어 아이템 획득 처리
- 전투 보상과 스테이지 진행 구조 연계

---

### 🗺️ 스테이지 시스템

📄 [Stage System](링크입력)

- Enemy 사망 이벤트 기반 생존 Enemy 수 관리
- 모든 Enemy 처치 시 다음 스테이지 진행
- 스테이지 진행에 따라 등장 Enemy 종류 및 수량 증가

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
