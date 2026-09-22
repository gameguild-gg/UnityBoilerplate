using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    /// <summary>
    /// Play Mode tests actually run the game loop. Anything that needs
    /// frames, coroutines, physics or live GameObjects goes here.
    /// They are slower than Edit Mode tests, so keep most logic in Edit Mode.
    /// </summary>
    public class PlayModeTests
    {
        [UnityTest]
        public IEnumerator Rigidbody_FallsUnderGravity()
        {
            var go = new GameObject("falling");
            go.AddComponent<Rigidbody>();
            var startY = go.transform.position.y;

            // Yield a few physics steps so gravity can move the object.
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

            Assert.Less(go.transform.position.y, startY);
            Object.Destroy(go); // clean up so other tests are not affected
        }

        [UnityTest]
        public IEnumerator WaitForSeconds_AdvancesGameTime()
        {
            var before = Time.time;
            yield return new WaitForSeconds(0.1f);
            Assert.GreaterOrEqual(Time.time - before, 0.1f);
        }
    }
}
