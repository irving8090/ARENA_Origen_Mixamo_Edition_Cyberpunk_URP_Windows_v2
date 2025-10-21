
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Unity.AI.Navigation;

public class ProjectInitializer : MonoBehaviour
{
    [MenuItem("Tools/Initialize ARENA/Build Mixamo Edition (Cyberpunk)")]
    public static void BuildMixamoEdition()
    {
        CreateMainMenu();
        CreateCharacterSelect();
        CreateScene("ZoneOfTrial", Color.black);
        CreateScene("AbandonedSector", Color.black);
        CreateScene("CoreNucleus", Color.black);
        EditorUtility.DisplayDialog("ARENA Init", "Mixamo-ready Cyberpunk prototype created. Run Tools->ARENA->Create Base AnimatorControllers and import Mixamo FBX.", "OK");
    }

    static void CreateMainMenu()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject cam = new GameObject("Main Camera"); cam.tag = "MainCamera"; cam.AddComponent<Camera>().transform.position = new Vector3(0,4,-6); cam.AddComponent<CameraFollow>();
        GameObject canvas = new GameObject("MainMenuCanvas"); var c = canvas.AddComponent<Canvas>(); c.renderMode = RenderMode.ScreenSpaceOverlay; canvas.AddComponent<UnityEngine.UI.CanvasScaler>(); canvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        var titleGO = new GameObject("Title"); titleGO.transform.SetParent(canvas.transform); var txt = titleGO.AddComponent<UnityEngine.UI.Text>(); txt.text = "ARENA: Origen"; txt.fontSize = 56; txt.alignment = TextAnchor.UpperCenter; txt.color = Color.red;
        var mm = new GameObject("MenuManager").AddComponent<MenuManager>();
        var playBtn = CreateUIButton("PlayButton", canvas.transform, "Jugar", new Vector2(0, -120)); playBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(mm.PlayGame);
        var quitBtn = CreateUIButton("QuitButton", canvas.transform, "Salir", new Vector2(0, -200)); quitBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(mm.QuitGame);
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/MainMenu.unity");
    }

    static void CreateCharacterSelect()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject cam = new GameObject("Main Camera"); cam.tag = "MainCamera"; cam.AddComponent<Camera>().transform.position = new Vector3(0,4,-6); cam.AddComponent<CameraFollow>();
        GameObject canvas = new GameObject("CharSelectCanvas"); var c = canvas.AddComponent<Canvas>(); c.renderMode = RenderMode.ScreenSpaceOverlay; canvas.AddComponent<UnityEngine.UI.CanvasScaler>(); canvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        var csm = new GameObject("CharacterSelectManager").AddComponent<CharacterSelectManager>();
        var kaelBtn = CreateUIButton("KaelButton", canvas.transform, "Kael - Cuerpo a Cuerpo", new Vector2(-140, -100)); kaelBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(csm.ChooseKael);
        var lyraBtn = CreateUIButton("LyraButton", canvas.transform, "Lyra - A Distancia", new Vector2(140, -100)); lyraBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(csm.ChooseLyra);
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/CharacterSelect.unity");
    }

    static void CreateScene(string sceneName, Color ambient)
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = sceneName;
        GameObject cam = new GameObject("Main Camera"); cam.tag = "MainCamera"; var cameraComp = cam.AddComponent<Camera>(); cameraComp.transform.position = new Vector3(0,6,-8); cameraComp.transform.rotation = Quaternion.Euler(20,0,0); cam.AddComponent<CameraFollow>();
        GameObject light = new GameObject("Directional Light"); var lightComp = light.AddComponent<Light>(); lightComp.type = LightType.Directional; lightComp.transform.rotation = Quaternion.Euler(50,-30,0);
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane); ground.name = "Ground"; ground.transform.localScale = new Vector3(6,1,6);
        // Create player placeholder
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule); player.name = "Player"; player.tag = "Player"; player.transform.position = new Vector3(0,1,0); player.AddComponent<CharacterController>(); player.AddComponent<PlayerController>(); player.AddComponent<AnimationDamage>();
        // Add HUD canvas
        GameObject hud = new GameObject("HUD"); var canvas = hud.AddComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay; hud.AddComponent<UnityEngine.UI.CanvasScaler>(); hud.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        var hudScript = hud.AddComponent<UI.HUDController>();
        // Add TextMeshPro placeholders if TextMeshPro installed, else default Text is used
        var healthGO = new GameObject("HealthText"); healthGO.transform.SetParent(hud.transform); var healthTxt = healthGO.AddComponent<UnityEngine.UI.Text>(); healthTxt.rectTransform.anchoredPosition = new Vector2(10,-10); hudScript.healthText = healthTxt as TMPro.TextMeshProUGUI;
        // Create enemies placeholders
        for (int i=0;i<3;i++)
        {
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule); enemy.name = "Enemy_"+i; enemy.transform.position = new Vector3(Random.Range(-8,8),1,Random.Range(-8,8));
            enemy.AddComponent<EnemyAI>(); enemy.AddComponent<EnemyHealth>(); enemy.AddComponent<UnityEngine.AI.NavMeshAgent>(); enemy.AddComponent<Animator>();
        }
        // NavMeshSurface
        var navGO = new GameObject("NavMeshSurface"); var surface = navGO.AddComponent<Unity.AI.Navigation.NavMeshSurface>(); surface.collectObjects = Unity.AI.CollectObjects.All; surface.BuildNavMesh();
        // Pause menu
        GameObject pause = new GameObject("PauseMenu"); pause.transform.SetParent(hud.transform); var pm = pause.AddComponent<UnityEngine.UI.Image>(); pause.SetActive(false);
        var pauseManager = hud.AddComponent<PauseManager>(); pauseManager.pauseMenu = pause;
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/"+sceneName+".unity");
    }

    static GameObject CreateUIButton(string name, Transform parent, string label, UnityEngine.Vector2 anchoredPosition)
    {
        GameObject btnGO = new GameObject(name); btnGO.transform.SetParent(parent);
        var img = btnGO.AddComponent<UnityEngine.UI.Image>(); var btn = btnGO.AddComponent<UnityEngine.UI.Button>(); var rt = btnGO.GetComponent<RectTransform>(); rt.sizeDelta = new UnityEngine.Vector2(260,40); rt.anchoredPosition = anchoredPosition;
        var txtGO = new GameObject("Text"); txtGO.transform.SetParent(btnGO.transform); var txt = txtGO.AddComponent<UnityEngine.UI.Text>(); txt.text = label; txt.alignment = TextAnchor.MiddleCenter; txt.rectTransform.sizeDelta = rt.sizeDelta; txt.color = Color.red;
        return btnGO;
    }
}
#endif
