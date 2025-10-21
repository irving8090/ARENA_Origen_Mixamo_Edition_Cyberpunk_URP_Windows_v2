
ARENA: Origen — Mixamo Edition (Cyberpunk)
=========================================

CONTENIDO
- Proyecto preparado para importar personajes y animaciones desde Mixamo.
- Herramientas editoriales: Tools -> Initialize ARENA -> Build Mixamo Edition (Cyberpunk)
- Generador de AnimatorControllers: Tools -> ARENA -> Create Base AnimatorControllers

REQUISITOS
- Unity 2022.3 LTS recomendado
- Package: AI Navigation (para NavMeshSurface)
- TextMeshPro package (opcional, HUD uses TMP if available)

PASOS RÁPIDOS PARA EMPEZAR
1) Descomprime el zip en una carpeta.
2) Abre Unity Hub -> Add -> selecciona la carpeta del proyecto.
3) Abre el proyecto con Unity 2022.3.
4) En Unity: Window -> Package Manager -> instala 'AI Navigation' (si no está).
5) Menú: Tools -> Initialize ARENA -> Build Mixamo Edition (Cyberpunk).
6) Menú: Tools -> ARENA -> Create Base AnimatorControllers (esto crea Kael_Controller, Lyra_Controller, Enemy_Controller en Assets/Animators).
7) Abre Assets/Scenes/MainMenu.unity y presiona Play para probar el menú y selección.

IMPORTAR PERSONAJES DESDE MIXAMO (PASO A PASO)
A) En Mixamo:
 - Selecciona personaje y elige 'FBX for Unity' con 'Skin: With Skin' y FPS=30.
 - Descarga los clips: Idle, Run, Attack (o Shoot), Death.

B) En Unity:
 - Crea carpetas: Assets/Characters/Kael y Assets/Characters/Lyra.
 - Copia los FBX a esas carpetas.
 - Selecciona cada FBX -> Inspector -> Rig -> Animation Type: Humanoid -> Apply.
 - En Model -> Configure... verifica huesos.

C) AnimatorController:
 - Ve a Assets/Animators y abre Kael_Controller (si lo creaste con la herramienta).
 - Arrastra tus clips a los estados: Idle, Run, Attack, Die.
 - Parámetros: Speed (float), Attack (trigger), Die (trigger).
 - Repite para Lyra_Controller y Enemy_Controller.

D) Prefab y configuración en escena:
 - Reemplaza el 'Player' placeholder en la escena por tu modelo Kael/Lyra (arrástralo a la jerarquía en la posición del placeholder).
 - Añade CharacterController al root del prefab (Component -> Physics -> CharacterController).
 - Añade PlayerController, AnimationDamage y asigna referencias (meleePrefab/rangedPrefab si tienes).
 - En Animator del prefab, asigna Kael_Controller o Lyra_Controller.
 - Para el ataque melee: abre tu clip Attack, en el frame de impacto agrega un Animation Event que llame a 'DoMeleeDamage'.

E) Enemigos:
 - Importa el FBX enemigo desde Mixamo y configura Rig: Humanoid -> Apply.
 - Crea AnimatorController (o usa Enemy_Controller creado) y pon los clips Idle/Run/Attack/Die.
 - Reemplaza los enemigos placeholders por tu prefab enemigo. Asegúrate que tenga:
   - NavMeshAgent
   - EnemyAI.cs
   - EnemyHealth.cs
   - Animator con Enemy_Controller

F) Bake NavMesh:
 - Window -> AI -> Navigation -> Bake (o usa NavMeshSurface component -> Build).
 - Revisa que los NavMeshAgents se muevan correctamente.

G) Probar:
 - Abre MainMenu.unity, Play -> Jugar -> selecciona Kael o Lyra -> prueba movimientos y ataques.
 - Kael: clic izquierdo inicia Attack trigger y la animación debe generar el evento DoMeleeDamage.
 - Lyra: clic izquierdo puede activar Attack trigger; puedes enlazar RangedWeapon.Fire() en el animator o usar input.

TECLAS (resumen)
- W A S D: mover
- Espacio: saltar
- Shift izquierdo: correr
- Click izquierdo: atacar / disparar
- E: interactuar
- Esc: pausar / reanudar

NOTAS FINALES
- Los AnimatorControllers creados por la herramienta NO tienen clips; debes asignar tus clips de Mixamo.
- Si quieres, puedo preparar también ejemplos de clips libres y los importo por ti (si me los das).
- Cuando reemplaces placeholders por modelos, ajusta el tamaño/scale (Mixamo FBX suelen venir a escala correcta, prueba uniformemente).

