using ConditionalLog;
using UnityEngine;
using UnityEngine.UI;

namespace ConditionalLog.Demo
{
    public sealed class DemoGame : MonoBehaviour
    {
        [SerializeField]
        private float _obstacleSpeed = 6f;

        [SerializeField]
        private float _spawnInterval = 1.4f;

        [SerializeField]
        private float _jumpVelocity = 12f;

        private DemoRunner _runner;
        private DemoSpawner _spawner;
        private Transform _obstaclesRoot;
        private GameObject _obstacleTemplate;
        private Vector3 _runnerStart;
        private Text _hud;
        private Text _filterHint;
        private string _hudKey = "demo.hud.start";

        public bool IsPlaying { get; private set; }

        public float ObstacleSpeed => _obstacleSpeed;

        private void Awake()
        {
            EnsureWorld();
        }

        private void Start()
        {
            ConditionalLogLocale.LanguageChanged += OnLanguageChanged;
            ShowHudKey("demo.hud.start");
            Log.Info("Hud", DemoStrings.T("demo.log.press_start"));
        }

        private void OnDestroy()
        {
            ConditionalLogLocale.LanguageChanged -= OnLanguageChanged;
        }

        private void OnLanguageChanged()
        {
            RefreshLocaleUi();
        }

        private void RefreshLocaleUi()
        {
            if (_filterHint != null)
            {
                _filterHint.text = DemoStrings.T("demo.hint_f1");
            }

            if (!IsPlaying && !string.IsNullOrEmpty(_hudKey))
            {
                ShowHudKey(_hudKey);
            }
        }

        private void Update()
        {
            if (IsPlaying)
            {
                return;
            }

            if (DemoInput.JumpPressedThisFrame())
            {
                BeginRun();
            }
        }

        public void NotifyHit()
        {
            if (!IsPlaying)
            {
                return;
            }

            IsPlaying = false;
            Log.Warning("Obstacle", DemoStrings.T("demo.log.collision"));
            Log.Error("Hud", DemoStrings.T("demo.log.game_over"));
            Log.Info("Hud", DemoStrings.T("demo.log.press_restart"));
            ShowHudKey("demo.hud.game_over");
        }

        private void BeginRun()
        {
            ClearObstacles();
            _runner.ResetPose(_runnerStart);
            IsPlaying = true;
            _hudKey = string.Empty;
            ShowHud(string.Empty);
            Log.Progress("Boot", DemoStrings.T("demo.log.run_start"));
        }

        private void ShowHudKey(string key)
        {
            _hudKey = key ?? string.Empty;
            ShowHud(string.IsNullOrEmpty(_hudKey) ? string.Empty : DemoStrings.T(_hudKey));
        }

        private void ShowHud(string message)
        {
            if (_hud == null)
            {
                return;
            }

            _hud.text = message;
            _hud.enabled = !string.IsNullOrEmpty(message);
        }

        private void ClearObstacles()
        {
            for (int i = _obstaclesRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(_obstaclesRoot.GetChild(i).gameObject);
            }
        }

        private void EnsureWorld()
        {
            if (_runner != null)
            {
                return;
            }

            Sprite fallback = CreateWhiteSprite();
            Sprite groundSprite = LoadRequiredSprite("Assets/brackeys_platformer_assets/sprites/world_tileset.png", "world_tileset_0", fallback);
            Sprite runnerSprite = LoadRequiredSprite("Assets/brackeys_platformer_assets/sprites/knight.png", "knight_0", fallback);
            Sprite treeCrown = LoadRequiredSprite("Assets/brackeys_platformer_assets/sprites/world_tileset.png", "world_tileset_36", fallback);
            Sprite treeTrunk = LoadRequiredSprite("Assets/brackeys_platformer_assets/sprites/world_tileset.png", "world_tileset_44", fallback);

            GameObject ground = CreateBox("Ground", new Vector3(0f, -3.5f, 0f), groundSprite, new Color(0.35f, 0.35f, 0.35f), false, 20, new Vector2(24f, 1f));
            ground.transform.SetParent(transform, true);

            GameObject runnerGo = CreateBox("Runner", new Vector3(-5f, -2f, 0f), runnerSprite, Color.white, false, 10);
            runnerGo.GetComponent<BoxCollider2D>().size = new Vector2(0.75f, 1.5f);
            runnerGo.transform.SetParent(transform, true);
            Rigidbody2D body = runnerGo.AddComponent<Rigidbody2D>();
            body.freezeRotation = true;
            body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            body.gravityScale = 3.5f;
            _runner = runnerGo.AddComponent<DemoRunner>();
            _runner.Configure(this, _jumpVelocity);
            _runnerStart = runnerGo.transform.position;

            _obstaclesRoot = new GameObject("Obstacles").transform;
            _obstaclesRoot.SetParent(transform, false);

            _obstacleTemplate = CreateBox("ObstacleTemplate", new Vector3(10f, -2.5f, 0f), treeCrown, Color.white, true, 0);
            _obstacleTemplate.transform.SetParent(transform, true);
            _obstacleTemplate.AddComponent<DemoObstacle>();
            _obstacleTemplate.SetActive(false);

            GameObject spawnerGo = new GameObject("Spawner");
            spawnerGo.transform.SetParent(transform, false);
            spawnerGo.transform.position = new Vector3(10f, -2.5f, 0f);
            _spawner = spawnerGo.AddComponent<DemoSpawner>();
            _spawner.Configure(this, _obstacleTemplate, _obstaclesRoot, treeCrown, treeTrunk, _spawnInterval);

            Camera cam = Camera.main;
            if (cam == null)
            {
                Log.Warning("Boot", DemoStrings.T("demo.log.no_camera"));
            }
            else
            {
                cam.orthographic = true;
                cam.orthographicSize = 5f;
                cam.transform.position = new Vector3(0f, 0f, -10f);
            }

            _hud = CreateHud();
        }

        private Text CreateHud()
        {
            GameObject canvasGo = new GameObject("HudCanvas");
            canvasGo.transform.SetParent(transform, false);
            Canvas canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGo.AddComponent<CanvasScaler>();
            canvasGo.AddComponent<GraphicRaycaster>();

            Font font = OsOrBuiltinFont.Resolve();
            if (font == null)
            {
                Log.Error("Hud", DemoStrings.T("demo.log.no_font"));
            }

            GameObject hintGo = new GameObject("FilterHint");
            hintGo.transform.SetParent(canvasGo.transform, false);
            Text hint = hintGo.AddComponent<Text>();
            hint.font = font;
            hint.fontSize = 22;
            hint.alignment = TextAnchor.UpperCenter;
            hint.color = new Color(1f, 1f, 1f, 0.9f);
            hint.horizontalOverflow = HorizontalWrapMode.Overflow;
            hint.verticalOverflow = VerticalWrapMode.Overflow;
            hint.supportRichText = false;
            hint.text = DemoStrings.T("demo.hint_f1");
            _filterHint = hint;
            RectTransform hintRect = hint.rectTransform;
            hintRect.anchorMin = new Vector2(0f, 1f);
            hintRect.anchorMax = new Vector2(1f, 1f);
            hintRect.pivot = new Vector2(0.5f, 1f);
            hintRect.anchoredPosition = new Vector2(0f, -16f);
            hintRect.sizeDelta = new Vector2(0f, 36f);

            GameObject textGo = new GameObject("Message");
            textGo.transform.SetParent(canvasGo.transform, false);
            Text text = textGo.AddComponent<Text>();
            text.font = font;
            text.fontSize = 36;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.supportRichText = false;

            RectTransform rect = text.rectTransform;
            rect.anchorMin = new Vector2(0.5f, 0.55f);
            rect.anchorMax = new Vector2(0.5f, 0.55f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(900f, 200f);
            return text;
        }

        private static GameObject CreateBox(
            string name,
            Vector3 position,
            Sprite sprite,
            Color fallbackColor,
            bool isTrigger,
            int sortingOrder,
            Vector2 tiledSize = default)
        {
            GameObject go = new GameObject(name);
            go.transform.position = position;

            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.color = sprite.name == "white" ? fallbackColor : Color.white;
            renderer.sortingOrder = sortingOrder;

            BoxCollider2D box = go.AddComponent<BoxCollider2D>();
            box.isTrigger = isTrigger;

            if (tiledSize.x > 0f && tiledSize.y > 0f)
            {
                renderer.drawMode = SpriteDrawMode.Tiled;
                renderer.size = tiledSize;
                box.size = tiledSize;
            }
            else
            {
                box.size = sprite.bounds.size;
            }

            return go;
        }

        private static Sprite LoadRequiredSprite(string assetPath, string spriteName, Sprite fallback)
        {
            Sprite sprite = LoadSprite(assetPath, spriteName);
            if (sprite != null)
            {
                return sprite;
            }

            Log.Warning("Boot", DemoStrings.T("demo.log.missing_sprite", spriteName));
            return fallback;
        }

        private static Sprite LoadSprite(string assetPath, string spriteName)
        {
#if UNITY_EDITOR
            UnityEngine.Object[] assets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(assetPath);
            if (assets == null)
            {
                return null;
            }

            for (int i = 0; i < assets.Length; i++)
            {
                if (assets[i] is Sprite sprite && sprite.name == spriteName)
                {
                    return sprite;
                }
            }
#endif
            return null;
        }

        private static Sprite CreateWhiteSprite()
        {
            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            texture.filterMode = FilterMode.Point;
            Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            sprite.name = "white";
            return sprite;
        }
    }

    internal static class DemoInput
    {
        public static bool JumpPressedThisFrame()
        {
            UnityEngine.InputSystem.Keyboard keyboard = UnityEngine.InputSystem.Keyboard.current;
            if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame)
            {
                return true;
            }

            UnityEngine.InputSystem.Mouse mouse = UnityEngine.InputSystem.Mouse.current;
            return mouse != null && mouse.leftButton.wasPressedThisFrame;
        }
    }
}
