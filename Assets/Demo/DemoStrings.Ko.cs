using System.Collections.Generic;

namespace ConditionalLog.Demo
{
    /// <summary>
    /// Optional Korean overlays for playground demo strings (not a logging i18n template).
    /// Delete this file (and the Korean branch in <see cref="DemoStrings"/>) if unused.
    /// </summary>
    public static class DemoStringsKo
    {
        static readonly Dictionary<string, string> Table = new Dictionary<string, string>
        {
            { "demo.hint_f1", "F1 — 로그 필터" },
            { "demo.hud.start", "Space로 시작" },
            { "demo.hud.game_over", "게임 오버\nSpace로 다시 시작" },
            { "demo.log.press_start", "Space로 시작" },
            { "demo.log.collision", "충돌" },
            { "demo.log.game_over", "게임 오버" },
            { "demo.log.press_restart", "Space로 다시 시작" },
            { "demo.log.run_start", "런 시작" },
            { "demo.log.no_camera", "메인 카메라 없음" },
            { "demo.log.no_font", "내장 폰트 없음" },
            { "demo.log.spawn_skipped", "스폰 생략: 템플릿 또는 스프라이트 없음" },
            { "demo.log.spawn_height", "스폰 높이={0}" },
            { "demo.log.jump_no_rb", "점프 생략: Rigidbody 없음" },
            { "demo.log.jump", "점프" },
            { "demo.log.missing_sprite", "스프라이트 없음 {0}" },
        };

        public static bool TryGet(string key, out string value) => Table.TryGetValue(key, out value);
    }
}
