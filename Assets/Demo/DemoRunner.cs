using ConditionalLog;
using UnityEngine;

namespace ConditionalLog.Demo
{
    public sealed class DemoRunner : MonoBehaviour
    {
        private DemoGame _game;
        private Rigidbody2D _body;
        private float _jumpVelocity;
        private bool _grounded;

        public void Configure(DemoGame game, float jumpVelocity)
        {
            _game = game;
            _jumpVelocity = jumpVelocity;
            _body = GetComponent<Rigidbody2D>();
        }

        public void ResetPose(Vector3 position)
        {
            transform.position = position;
            if (_body != null)
            {
                _body.linearVelocity = Vector2.zero;
            }

            _grounded = false;
        }

        private void Update()
        {
            if (_game == null || !_game.IsPlaying)
            {
                return;
            }

            if (!_grounded || !DemoInput.JumpPressedThisFrame())
            {
                return;
            }

            if (_body == null)
            {
                Log.Error("Runner", DemoStrings.T("demo.log.jump_no_rb"));
                return;
            }

            _body.linearVelocity = new Vector2(_body.linearVelocity.x, _jumpVelocity);
            _grounded = false;
            Log.Info("Runner", DemoStrings.T("demo.log.jump"));
        }

        private void FixedUpdate()
        {
            if (_body == null || _game == null || !_game.IsPlaying)
            {
                return;
            }

            if (_body.linearVelocity.y < 0f)
            {
                _body.linearVelocity += Vector2.up * (Physics2D.gravity.y * (_body.gravityScale * 0.6f) * Time.fixedDeltaTime);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.collider.GetComponent<DemoObstacle>() != null)
            {
                return;
            }

            _grounded = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<DemoObstacle>() == null)
            {
                return;
            }

            _game.NotifyHit();
        }
    }
}
