# Project_Urban - Unity Card Battler

## Architecture Overview

**Genre**: Turn-based card battler with elemental mechanics (Flame, Ice, Grass)  
**Game Flow**: MainScene → ElementSelectScene → MapScene (store/shelter) → BattleScene

### Core Managers (Singleton Pattern)
- **GameManager** (`Singleton<T>`): Persists across scenes via `DontDestroyOnLoad`. Stores player health, deck, coin, selected element
- **BattleManager** (`SceneSingleton<T>`): Per-scene singleton. Manages turn flow via `UnityEvent`s: `OnTurnStart`, `OnTurnEnd`, `OnBattleEnd`
- **CardManager**: Creates card instances from `CardData` ScriptableObject, manages card sprite/prefab loading

### Key Systems

**Card System**: Enum-based identification via `CardName` (0-99: None, 100-199: Flame, 200-299: Ice, 300-399: Grass). Element derived by range in `Util.GetElement()`. All cards inherit from abstract `Card` class with abstract `Use(Target)` method.

**Target-Damage Model**: Abstract `Target` class (inherited by `Player`/`Enemy`). Damage flows through `Protect` (shield) then `HP`. Status effects stored in `conditionStatusList`.

**Deck Management**: 
- `Deck` class tracks three lists: `originCardList`, `unusedCardList`, `usedCardList`
- Turn cycle: Draw 6 cards → Play cards → `DiscardAll()` → Enemy turn

**Input Handling**: `MousePointer` publishes `UnityEvent`s (`OnCardSelect`, `OnEnemySelect`, etc.). `Player` subscribes in `Awake()` to wire up gameplay logic.

## Code Conventions

### File Organization
```
Assets/
  01_Scenes/     # .unity scene files
  02_Scripts/    # All C# scripts
  03_Images/     # Sprites
  04_Sounds/     # Audio
  05_Prefabs/    # Prefabs (Cards/, etc.)
  06_Data/       # ScriptableObjects
```

### Naming Patterns
- **Korean comments**: Most inline comments and variable names use Korean (e.g., `// 시작할때 카드 6장 드로우`)
- **SerializeField**: Prefer `[SerializeField] private` for inspector-exposed fields
- **No namespaces**: Project uses global namespace for all game code
- **Enum ranges**: Use numeric ranges for categorization (see `CardName` enum: 0-99 for none element, 100-199 for Flame, etc.)

### Unity Patterns
- **View-Controller separation**: Controllers (`HealthController`, `CostController`) expose `UnityEvent`s. Views (`HealthView`, `CostView`) bind via `.Bind()` method
- **Event wiring**: Always wire events in `Awake()`, access managers in `Start()` (ensures singleton initialization order)
- **ScriptableObject data**: `CardData` uses `OnEnable()` to build lookup dictionaries (`elementMap`, `cardNameMap`) from serialized lists

## Critical Workflows

### Adding a New Card
1. Add enum to `CardName.cs` (respect element range: 0-99/100-199/200-299/300-399)
2. Create script in `Assets/02_Scripts/Card/Concrete/` inheriting appropriate base (`Attack`/`Defense`/`Buff`/`Debuff`)
3. Add entry to `Assets/CardData.json` with Korean name and description
4. Create prefab in `Assets/05_Prefabs/Cards/[Element]/` 
5. Assign to `CardData` ScriptableObject's list

### Scene Progression
Element selection saves bonus cards via `GameManager.SetBonusCards()`, which initializes the `Deck` from `initialDeckRecipe` + bonus cards. Battle victory awards coins via `BattleManager.AddCoin()`.

### Turn Logic
`BattleManager.TurnEnd()` triggers:
1. `OnTurnEnd` event → `Player.HandleTurnEnd()` → `Deck.DiscardAll()`  
2. Enemy actions execute via `EnemyManager.ExecuteEnemyAction(callback)`  
3. Callback triggers `OnTurnStart` → `Player.HandleTurnStart()` → Draw 6 cards

## Testing & Debugging
- **Play from MainScene**: Game expects proper scene flow; playing from BattleScene directly may cause null refs on `GameManager.Deck`
- **Card costs**: Debug logs use format `코스트 부족 - 카드 코스트:{card.Cost}/현재 코스트:{Cost.CurrentCost}`
- **No automated tests**: No Unity Test Framework setup; manual play-testing required

## Assets & Dependencies
- **TextMeshPro**: Used for all UI text (`TMP_Text`, `TMP_Dropdown`)
- **Unity Input System**: Input actions defined in `.inputactions` files (MousePointer, InputSystem_Actions)
- **Third-party UI**: "Dark - Complete Horror UI" asset included but isolated in own namespace (`Michsky.UI.Dark`)

## Common Pitfalls
- **Singleton timing**: Access `BattleManager.Instance` only after scene loads (in `Start()`, not `Awake()`)
- **Element derivation**: Never hardcode element—always use `Util.GetElement(cardName)` to respect enum ranges
- **Card selection**: Defense/Buff cards target player (double-click to use), Attack/Debuff cards require enemy target
