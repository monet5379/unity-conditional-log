using ConditionalLog;
using UnityEngine;

namespace ConditionalLog.Demo
{
    public sealed class DemoSpawner : MonoBehaviour
    {
        private DemoGame _game;
        private GameObject _template;
        private Transform _root;
        private Sprite _crown;
        private Sprite _trunk;
        private float _interval;
        private float _elapsed;

        public void Configure(DemoGame game, GameObject template, Transform root, Sprite crown, Sprite trunk, float interval)
        {
            _game = game;
            _template = template;
            _root = root;
            _crown = crown;
            _trunk = trunk;
            _interval = interval;
        }

        private void Update()
        {
            if (_game == null || !_game.IsPlaying)
            {
                return;
            }

            _elapsed += Time.deltaTime;
            if (_elapsed < _interval)
            {
                return;
            }

            _elapsed = 0f;
            Spawn();
        }

        private void Spawn()
        {
            if (_template == null || _crown == null || _trunk == null)
            {
                Log.Error("Obstacle", DemoStrings.T("demo.log.spawn_skipped"));
                return;
            }

            int heightTiles = Random.Range(1, 3);
            GameObject instance = Instantiate(_template, transform.position, Quaternion.identity, _root);
            instance.SetActive(true);
            DemoObstacle obstacle = instance.GetComponent<DemoObstacle>();
            obstacle.Bind(_game);
            obstacle.ApplyHeight(heightTiles, _crown, _trunk);
            Log.Info("Obstacle", DemoStrings.T("demo.log.spawn_height", heightTiles));
        }
    }
}
