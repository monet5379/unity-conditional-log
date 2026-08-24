using UnityEngine;

namespace ConditionalLog.Demo
{
    public sealed class DemoObstacle : MonoBehaviour
    {
        private DemoGame _game;

        private void Awake()
        {
            _game = GetComponentInParent<DemoGame>();
        }

        public void Bind(DemoGame game)
        {
            _game = game;
        }

        public void ApplyHeight(int heightTiles, Sprite crown, Sprite trunk)
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            if (heightTiles <= 1)
            {
                renderer.sprite = crown;
                box.size = Vector2.one;
                box.offset = Vector2.zero;
                return;
            }

            renderer.sprite = trunk;
            box.size = new Vector2(1f, 2f);
            box.offset = new Vector2(0f, 0.5f);

            GameObject crownGo = new GameObject("Crown");
            crownGo.transform.SetParent(transform, false);
            crownGo.transform.localPosition = new Vector3(0f, 1f, 0f);
            SpriteRenderer crownRenderer = crownGo.AddComponent<SpriteRenderer>();
            crownRenderer.sprite = crown;
            crownRenderer.sortingOrder = renderer.sortingOrder;
        }

        private void Update()
        {
            if (_game == null || !_game.IsPlaying)
            {
                return;
            }

            transform.position += Vector3.left * (_game.ObstacleSpeed * Time.deltaTime);
            if (transform.position.x < -12f)
            {
                Destroy(gameObject);
            }
        }
    }
}
