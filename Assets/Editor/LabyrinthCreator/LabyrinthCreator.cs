using UnityEngine;
using UnityEditor;
using System.Collections;
using SlideSeparator; //BB 20190910
using System.Collections.Generic; //BB 20210103
using TMPro; //BB 20210103

class MazeBuilder : EditorWindow {
    public string language = "EN";
    public string defaultSlideBackground = "marble-black.jpg"; //to be set from the XML global settings later in lab construction 
	public string illumination = "Lamps"; 
	public string showDoorLock = "Yes";
    public string showSlideFrames = "Yes";
    public string showPlants = "Yes";
    public float ceilingTiling = 1.0f;
    public float floorTiling = 1.0f;

    //private List<GameObject> tilesWordSoup = new List<GameObject>(); //??
    //public static List<string> dictionaryWordSoup = new List<string>(); //??
    private const int HEIGHT = 5; //BB 20210103
    private const int WIDTH = 8;

    [MenuItem("Maze Builder/Create from XML")] 

	static void Init() {
		MazeBuilder window = (MazeBuilder)EditorWindow.GetWindow(typeof(MazeBuilder));
		window.Show();
	} 	

	//create one object from prefab
	private GameObject createObject(GameObject original, string name)
	{
		Object newObject;

		//check if prefab exist
		if (PrefabUtility.GetPrefabType(original).ToString()!="None")
		{
			newObject =  PrefabUtility.InstantiatePrefab(original);
		}
		else
		{
			newObject =  Instantiate(original,Vector3.zero,Quaternion.identity);
		}

		((GameObject)newObject).transform.position = new Vector3 (0, 0, 0);
		newObject.name = name;

		return (GameObject)newObject;
	}

	void OnGUI()
	{
		//defaultSlideBackground = "marble-black.jpg";

		if (GUILayout.Button ("Import Textures")) {
			
			string path = EditorUtility.OpenFolderPanel("Select path for textures to import", "", "");
			if (path != "") {
				try {
					string[] files = System.IO.Directory.GetFiles(path);
					for (int i = 0; i < files.Length; i++) {
						System.IO.File.Copy(path + "/" + System.IO.Path.GetFileName (files[i]), "Assets/Materials/Textures/" + System.IO.Path.GetFileName (files[i]), true);
						AssetDatabase.ImportAsset ("Assets/Materials/Textures/" + System.IO.Path.GetFileName (files[i]), ImportAssetOptions.ForceUpdate);
					}
				}
				catch(System.Exception ) {
					UnityEditor.EditorUtility.DisplayDialog ("Error","Error copying files.", "Ok", "");
				}


			}
		}

		if (GUILayout.Button ("Import Audio")) {

			string path = EditorUtility.OpenFolderPanel("Select path for audio files to import", "", "");
			if (path != "") {
				try {
					string[] files = System.IO.Directory.GetFiles(path);
					for (int i = 0; i < files.Length; i++) {
						System.IO.File.Copy(path + "/" + System.IO.Path.GetFileName (files[i]), "Assets/Materials/Sounds/" + System.IO.Path.GetFileName (files[i]), true);
						AssetDatabase.ImportAsset ("Assets/Materials/Sounds/" + System.IO.Path.GetFileName (files[i]), ImportAssetOptions.ForceUpdate);
					}
				}
				catch(System.Exception ) {
					UnityEditor.EditorUtility.DisplayDialog ("Error","Error copying files.", "Ok", "");
				}


			}
		}

		if (GUILayout.Button ("Import XML")) {
			if (UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene ().rootCount > 0) {
				int answer = UnityEditor.EditorUtility.DisplayDialogComplex ("Warning", "All existing objects will be removed from the current scene.", "OK", "Cancel", "Keep Existing Objects");
				if (answer == 0) {

					GameObject[] allActiveOjects = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene ().GetRootGameObjects ();
					foreach (GameObject rootObject in allActiveOjects) {
						DestroyImmediate (rootObject);
					}
				} else if (answer == 1){
					return;
				}
			}

			AssemblyCSharpEditor.Labyrinth newLabyrinth = null;
			string path = EditorUtility.OpenFilePanel ("Select path for XML to import", "", "");
			if (path != "") {
				try {
					newLabyrinth = AssemblyCSharpEditor.Labyrinth.LoadXml (path);
				}
				catch(System.Exception e) {
					Debug.Log ("Error parsing XML, e.Message: " + e.Message);
					UnityEditor.EditorUtility.DisplayDialog ("Error parsing XML", e.Message, "OK");
				}
				createLabyrinth(newLabyrinth);
			}
		}

	}

    private string checkLabyrinthParameter(string origParameterValue, string newValue) 
    {
        if (newValue != null && newValue != "") {
            return newValue;
        }
        return origParameterValue;
    }

    public bool createLabyrinth(AssemblyCSharpEditor.Labyrinth newLabyrinth)
	{
		addGlobalGameManager ();
 
        //Debug.Log("GlobalSettings.DefaultSlideBackground = " + newLabyrinth.GlobalSettings.DefaultSlideBackground); //BB, 20171207
        //Debug.Log("Language:" + newLabyrinth.GlobalSettings.Language);
        language = checkLabyrinthParameter(language, newLabyrinth.GlobalSettings.Language);
        defaultSlideBackground = checkLabyrinthParameter(defaultSlideBackground, newLabyrinth.GlobalSettings.DefaultSlideBackground); //slides with back image different than defaultSlideBackground or NONE will ot be shown intil the camera will arroach them
        illumination = checkLabyrinthParameter(illumination, newLabyrinth.GlobalSettings.Illumination);
        showDoorLock = checkLabyrinthParameter(showDoorLock, newLabyrinth.GlobalSettings.ShowDoorLock);
        showSlideFrames = checkLabyrinthParameter(showSlideFrames, newLabyrinth.GlobalSettings.ShowSlideFrames);
        showPlants = checkLabyrinthParameter(showPlants, newLabyrinth.GlobalSettings.ShowPlants);
        ceilingTiling = newLabyrinth.GlobalSettings.CeilingTiling;
        floorTiling = newLabyrinth.GlobalSettings.FloorTiling;

        TextManager.Language = language; //BB, 20171208
        TextManager.GetInstance();
        //Debug.Log("Localization test:" + TextManager.Get("lastAnswer_____"));

        foreach(AssemblyCSharpEditor.Room room in newLabyrinth.Rooms)
		{
			GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/Room.prefab", typeof(GameObject));
			if (prefab != null) {
				createObject (prefab, room.Name);

				GameObject question = GameObject.Find ("/" + room.Name + "/Questions/QuestionN");
				if (room.DoorN != null && room.DoorN.Question != null) {
					// Set Text, copy material and set image
					question.SetActive (true);
					createQuestion (room.DoorN.Question, "QuestionN", room.Name, question);
												
				} else {
					question.SetActive (false);
				}

				question = GameObject.Find ("/" + room.Name + "/Questions/QuestionE");
				if (room.DoorE != null && room.DoorE.Question != null) {
					// Set Text, copy material and set image
					question.SetActive (true);
					createQuestion (room.DoorE.Question, "QuestionE", room.Name, question);

				} else {
					question.SetActive (false);
				}

				question = GameObject.Find ("/" + room.Name + "/Questions/QuestionW");
				if (room.DoorW != null && room.DoorW.Question != null) {
					// Set Text, copy material and set image
					question.SetActive (true);
					createQuestion (room.DoorW.Question, "QuestionW", room.Name, question);

				} else {
					question.SetActive (false);
				}

				question = GameObject.Find ("/" + room.Name + "/Questions/QuestionS");
				if (room.DoorS != null && room.DoorS.Question != null) {
					// Set Text, copy material and set image
					question.SetActive (true);
					createQuestion (room.DoorS.Question, "QuestionS", room.Name, question);

				} else {
					question.SetActive (false);
				}

				GameObject slide = GameObject.Find ("/" + room.Name + "/Slides/SlideN_1");
				if (room.SlideN_1 != null) {
					slide.SetActive (true);
					createSlide (room.SlideN_1, "SlideN_1", room.Name, slide);

				} else {
					slide.SetActive (false);
				}

				slide = GameObject.Find ("/" + room.Name + "/Slides/SlideN_2");
				if (room.SlideN_2 != null) {
					slide.SetActive (true);
					createSlide (room.SlideN_2, "SlideN_2", room.Name, slide);

				} else {
					slide.SetActive (false);
				}

				slide = GameObject.Find ("/" + room.Name + "/Slides/SlideE_1");
				if (room.SlideE_1 != null) {
					slide.SetActive (true);
					createSlide (room.SlideE_1, "SlideE_1", room.Name, slide);

				} else {
					slide.SetActive (false);
				}

				slide = GameObject.Find ("/" + room.Name + "/Slides/SlideE_2");
				if (room.SlideE_2 != null) {
					slide.SetActive (true);
					createSlide (room.SlideE_2, "SlideE_2", room.Name, slide);

				} else {
					slide.SetActive (false);
				}

				slide = GameObject.Find ("/" + room.Name + "/Slides/SlideW_1");
				if (room.SlideW_1 != null) {
					slide.SetActive (true);
					createSlide (room.SlideW_1, "SlideW_1", room.Name, slide);

				} else {
					slide.SetActive (false);
				}

				slide = GameObject.Find ("/" + room.Name + "/Slides/SlideW_2");
				if (room.SlideW_2 != null) {
					slide.SetActive (true);
					createSlide (room.SlideW_2, "SlideW_2", room.Name, slide);

				} else {
					slide.SetActive (false);
				}

				slide = GameObject.Find ("/" + room.Name + "/Slides/SlideS_1");
				if (room.SlideS_1 != null) {
					slide.SetActive (true);
					createSlide (room.SlideS_1, "SlideS_1", room.Name, slide);

				} else {
					slide.SetActive (false);
				}

				slide = GameObject.Find ("/" + room.Name + "/Slides/SlideS_2");
				if (room.SlideS_2 != null) {
					slide.SetActive (true);
					createSlide (room.SlideS_2, "SlideS_2", room.Name, slide);

				} else {
					slide.SetActive (false);
				}

				if (room.WallTexture != null && room.WallTexture != "") {
					//AssetDatabase.CopyAsset ("Assets/Materials/WallMaterial.mat", "Assets/Materials/" + room.Name + "_WallMaterial.mat");
					//Material newMaterial = (Material)AssetDatabase.LoadAssetAtPath ("Assets/Materials/" + room.Name + "_WallMaterial.mat", typeof(Material));
					//newMaterial.SetTexture (room.Name + "_WallMaterial", (Texture)AssetDatabase.LoadAssetAtPath ("Assets/Materials/Textures/" + room.WallTexture, typeof(Material)));

					setWallTexture ("WallN", room.Name, room.WallTexture);
					setWallTexture ("WallE", room.Name, room.WallTexture);
					setWallTexture ("WallW", room.Name, room.WallTexture);
					setWallTexture ("WallS", room.Name, room.WallTexture);
				}

				if (room.FloorTexture != null && room.FloorTexture != "") {

					GameObject floor = GameObject.Find ("/" + room.Name + "/Floor");
					Material tempMaterial = new Material(floor.GetComponent<MeshRenderer> ().sharedMaterial);
					tempMaterial.mainTexture = (Texture)AssetDatabase.LoadAssetAtPath ("Assets/Materials/Textures/" + room.FloorTexture, typeof(Texture));
					floor.GetComponent<MeshRenderer> ().sharedMaterial = tempMaterial;
                    floor.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(floorTiling, floorTiling); //BB, 20171203, setting tiling to the XML value
                    floor.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_BumpMap", new Texture2D(32,32)); //BB, 20171203, set a new Texture for ellimination of the wall16_Normap map
                    floor.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_OcclusionMap", new Texture2D(32, 32)); //BB, 20171203, set a new Texture for ellimination of the wall16_Ambient_Occlusion map
                }

				if (room.CeilingTexture != null && room.CeilingTexture != "") {

					GameObject ceiling = GameObject.Find ("/" + room.Name + "/Ceiling");
					Material tempMaterial = new Material(ceiling.GetComponent<MeshRenderer> ().sharedMaterial);
					tempMaterial.mainTexture = (Texture)AssetDatabase.LoadAssetAtPath ("Assets/Materials/Textures/" + room.CeilingTexture, typeof(Texture));
					ceiling.GetComponent<MeshRenderer> ().sharedMaterial = tempMaterial;
                    ceiling.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(ceilingTiling, ceilingTiling); //BB, 20171203, setting tiling to the XML value
					ceiling.GetComponent<MeshRenderer> ().sharedMaterial.SetTexture("_BumpMap", new Texture2D(32, 32)); //BB, 20171203, set a new Texture for ellimination of the wall16_Normap map
                    ceiling.GetComponent<MeshRenderer> ().sharedMaterial.SetTexture("_OcclusionMap", new Texture2D(32, 32)); //BB, 20171203, set a new Texture for ellimination of the wall16_Ambient_Occlusion map
				}

                if (room.StartingRoom)
                {
					setStartingRoom (room.Name);
				}

				addGameManager (room);

                createGameRollABall(room);
                //createGameOtherType(room);

				addHiddenObject (room);

				if (room.Map != null && room.Map != "") {
					GameObject map = GameObject.Find ("/" + room.Name + "/FloorMap");
					map.SetActive (true);
					Material tempMaterial = new Material (map.GetComponent<MeshRenderer> ().sharedMaterial);
					tempMaterial.mainTexture = (Texture)AssetDatabase.LoadAssetAtPath ("Assets/Materials/Textures/" + room.Map, typeof(Texture));
					map.GetComponent<MeshRenderer> ().sharedMaterial = tempMaterial;
				} else {
					GameObject map = GameObject.Find ("/" + room.Name + "/FloorMap");
					map.SetActive (false);
				}

                if (showPlants != "Yes") {
				    GameObject plants = GameObject.Find ("/" + room.Name + "/Plants"); //BB, 20171203, done by XML settings
				    plants.SetActive (false);
                }
                if (illumination != "Lamps") {
				    GameObject lamps = GameObject.Find ("/" + room.Name + "/Lamps"); //BB, 20171203
				    lamps.SetActive (false);
                }
				GameObject this_room = GameObject.Find ("/" + room.Name); //for Thrace game modifications, as follows:
                if (illumination == "Torches") {
				    GameObject torches_prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/Torches.prefab", typeof(GameObject));
				    if (torches_prefab != null) {
				    	GameObject torches = createObject (torches_prefab, "Torches");
				    	torches.transform.SetParent (this_room.transform);
				    	torches.transform.position = new Vector3 (0, 0, 0);	
				    }
                }
                GameObject al_prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/AmbientLights.prefab", typeof(GameObject));
				if (al_prefab != null) {
					GameObject al = createObject (al_prefab, "Ambient Lights");
					al.transform.SetParent (this_room.transform);
					al.transform.position = new Vector3 (0, 0, 0);	
				}	
				if (room.CeilingTexture == "NONE") {
					GameObject.Find ("/" + room.Name + "/Ceiling").SetActive (false); //BB, 20171203, hiding the NONE ceiling
				}
                
	            if (showDoorLock == "No") {
 				    GameObject.Find ("/" + room.Name + "/Walls/WallN/door/porta/maniglia").SetActive (false); //BB, 20171203, hiding the maniglias
				    GameObject.Find ("/" + room.Name + "/Walls/WallS/door/porta/maniglia").SetActive (false); //BB, 20171203, hiding the maniglias
				    GameObject.Find ("/" + room.Name + "/Walls/WallW/door/porta/maniglia").SetActive (false); //BB, 20171203, hiding the maniglias
				    GameObject.Find ("/" + room.Name + "/Walls/WallE/door/porta/maniglia").SetActive (false); //BB, 20171203, hiding the maniglias
                }
				//GameObject.Find ("/" + room.Name + "/Lights/Point light").color = "FFE2C5FF";
				setLights(room.Name);
			}
			else {
				return false;
			}
		}

		//Position rooms in 3D space
		ArrayList processedRoomsList = new ArrayList ();
		foreach (AssemblyCSharpEditor.Room room in newLabyrinth.Rooms) {

			// add the first room so that is stays in this initial position
			if (processedRoomsList.Count == 0) {
				processedRoomsList.Add (room.Name);
			}
			
			GameObject currentRoom = GameObject.Find ("/" + room.Name);

			if (room.DoorN != null) {
				GameObject doorCover = GameObject.Find ("/" + room.Name + "/Walls/WallN/DoorCover");
				doorCover.SetActive (false);
				if (room.DoorN.Question != null) {
					GameObject question = GameObject.Find ("/" + room.Name + "/Questions/QuestionN");
					GameObject door = GameObject.Find ("/" + room.Name + "/Walls/WallN/door");
					door.GetComponent<DoorOpenScript> ().Question = question;
					door.GetComponent<DoorOpenScript> ().RotationSide = DoorOpenScript.SideOfRotation.Right; //BB, to open the door from the player to the next room, 04.09.2019
					door.GetComponent<DoorOpenScript> ().Speed = 1F;
				}

				GameObject nextRoom = GameObject.Find ("/" + room.DoorN.Room);
				GameObject doorCoverNext = GameObject.Find ("/" + room.DoorN.Room + "/Walls/WallS/DoorCover");
				doorCoverNext.SetActive (false);
				GameObject doorNext = GameObject.Find ("/" + room.DoorN.Room + "/Walls/WallS/door");
				doorNext.SetActive (false);

				if (!processedRoomsList.Contains (room.DoorN.Room)) {
					nextRoom.transform.position = new Vector3 (currentRoom.transform.position.x, currentRoom.transform.position.y, currentRoom.transform.position.z + 30);
					processedRoomsList.Add (room.DoorN.Room);
				}
			} else {
				/* disable door to avoid raycasting issue */
				GameObject door = GameObject.Find ("/" + room.Name + "/Walls/WallN/door");
				door.SetActive (false);
			}

			if (room.DoorE != null) {
				GameObject doorCover = GameObject.Find ("/" + room.Name + "/Walls/WallE/DoorCover");
				doorCover.SetActive (false);
				if (room.DoorE.Question != null) {
					GameObject question = GameObject.Find ("/" + room.Name + "/Questions/QuestionE");
					GameObject door = GameObject.Find ("/" + room.Name + "/Walls/WallE/door");
					door.GetComponent<DoorOpenScript> ().Question = question;
					door.GetComponent<DoorOpenScript> ().RotationSide = DoorOpenScript.SideOfRotation.Right; //BB, to open the door from the player to the next room, 04.09.2019
					door.GetComponent<DoorOpenScript> ().Speed = 1F;
				}

				GameObject nextRoom = GameObject.Find ("/" + room.DoorE.Room);
				GameObject doorCoverNext = GameObject.Find ("/" + room.DoorE.Room + "/Walls/WallW/DoorCover");
				doorCoverNext.SetActive (false);
				GameObject doorNext = GameObject.Find ("/" + room.DoorE.Room + "/Walls/WallW/door");
				doorNext.SetActive (false);

				if (!processedRoomsList.Contains (room.DoorE.Room)) {
					nextRoom.transform.position = new Vector3 (currentRoom.transform.position.x + 30, currentRoom.transform.position.y, currentRoom.transform.position.z);
					processedRoomsList.Add (room.DoorE.Room);
				}
			} else {
				/* disable door to avoid raycasting issue */
				GameObject door = GameObject.Find ("/" + room.Name + "/Walls/WallE/door");
				door.SetActive (false);
			}

			if (room.DoorW != null) {
				GameObject doorCover = GameObject.Find ("/" + room.Name + "/Walls/WallW/DoorCover");
				doorCover.SetActive (false);
				if (room.DoorW.Question != null) {
					GameObject question = GameObject.Find ("/" + room.Name + "/Questions/QuestionW");
					GameObject door = GameObject.Find ("/" + room.Name + "/Walls/WallW/door");
					door.GetComponent<DoorOpenScript> ().Question = question;
					door.GetComponent<DoorOpenScript> ().RotationSide = DoorOpenScript.SideOfRotation.Right; //BB, to open the door from the player to the next room, 04.09.2019
					door.GetComponent<DoorOpenScript> ().Speed = 1F;
				}

				GameObject nextRoom = GameObject.Find ("/" + room.DoorW.Room);
				GameObject doorCoverNext = GameObject.Find ("/" + room.DoorW.Room + "/Walls/WallE/DoorCover");
				doorCoverNext.SetActive (false);
				GameObject doorNext = GameObject.Find ("/" + room.DoorW.Room + "/Walls/WallE/door");
				doorNext.SetActive (false);

				if (!processedRoomsList.Contains (room.DoorW.Room)) {
					nextRoom.transform.position = new Vector3 (currentRoom.transform.position.x - 30, currentRoom.transform.position.y, currentRoom.transform.position.z);
					processedRoomsList.Add (room.DoorW.Room);
				}
			} else {
				/* disable door to avoid raycasting issue */
				GameObject door = GameObject.Find ("/" + room.Name + "/Walls/WallW/door");
				door.SetActive (false);
			}

			if (room.DoorS != null) {
				GameObject doorCover = GameObject.Find ("/" + room.Name + "/Walls/WallS/DoorCover");
				doorCover.SetActive (false);
				if (room.DoorS.Question != null) {
					GameObject question = GameObject.Find ("/" + room.Name + "/Questions/QuestionS");
					GameObject door = GameObject.Find ("/" + room.Name + "/Walls/WallS/door");
					door.GetComponent<DoorOpenScript> ().Question = question;
					door.GetComponent<DoorOpenScript> ().RotationSide = DoorOpenScript.SideOfRotation.Right; //BB, to open the door from the player to the next room, 04.09.2019
					door.GetComponent<DoorOpenScript> ().Speed = 1F;
				}

				GameObject nextRoom = GameObject.Find ("/" + room.DoorS.Room);
				GameObject doorCoverNext = GameObject.Find ("/" + room.DoorS.Room + "/Walls/WallN/DoorCover");
				doorCoverNext.SetActive (false);
				GameObject doorNext = GameObject.Find ("/" + room.DoorS.Room + "/Walls/WallN/door");
				doorNext.SetActive (false);

				if (!processedRoomsList.Contains (room.DoorS.Room)) {
					nextRoom.transform.position = new Vector3 (currentRoom.transform.position.x, currentRoom.transform.position.y, currentRoom.transform.position.z - 30);
					processedRoomsList.Add (room.DoorS.Room);
				}
			} else {
				/* disable door to avoid raycasting issue */
				GameObject door = GameObject.Find ("/" + room.Name + "/Walls/WallS/door");
				door.SetActive (false);
			}
		}

		/*
		UnityEditor.SceneManagement.EditorSceneManager.SaveScene (UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene(), "Assets/Game.unity");
		EditorUtility.DisplayProgressBar("Build Progress", "Please wait until the lighting is built", 0.5f);
		UnityEditor.Lightmapping.giWorkflowMode = UnityEditor.Lightmapping.GIWorkflowMode.OnDemand;
		UnityEditor.Lightmapping.Bake ();
		EditorUtility.ClearProgressBar();
		UnityEditor.SceneManagement.EditorSceneManager.SaveScene (UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene(), "Assets/Game.unity");
		*/

		return true;

	}

	private void setLights(string roomName)
	{
		for(int i=0; i<=7; i++) 
		{
			string suffix;
			if (i == 0)
				suffix = "";
			else
				suffix = " (" + i.ToString() + ")";
			Light l = GameObject.Find ("/" + roomName + "/Lights/Point light" + suffix).GetComponent<Light> ();
			l.intensity = 0.9f;
			l.range = 33;
			l.color = Color.Lerp(Color.red, Color.yellow, 0.8f);
		}
	}

	private void createQuestion(AssemblyCSharpEditor.Question question, string questionName, string roomName, GameObject questionObject)
	{
		if (question.Text != null) {
			GameObject questionText = GameObject.Find ("/" + roomName + "/Questions/" + questionName + "/QuestionText");
			questionText.GetComponent<TextMesh> ().text = question.Text;
            GameObject lastAnswerText = GameObject.Find("/" + roomName + "/Questions/" + questionName + "/AnswerText"); //BB, 20171208
            lastAnswerText.GetComponent<TextMesh>().text = TextManager.Get("lastAnswer_____") + " ";
		}

		if (question.Image != null) {

			GameObject questionPlane = GameObject.Find ("/" + roomName + "/Questions/" + questionName + "/Plane");

			Material tempMaterial = new Material(questionPlane.GetComponent<MeshRenderer> ().sharedMaterial);
			tempMaterial.mainTexture = (Texture)AssetDatabase.LoadAssetAtPath ("Assets/Materials/Textures/" + question.Image, typeof(Texture));
			questionPlane.GetComponent<MeshRenderer> ().sharedMaterial = tempMaterial;
		}

		if (question.Answer != null) {
			questionObject.GetComponent<ShowQuestion> ().Answer = question.Answer;
		}
        
	}

	private void createSlide(AssemblyCSharpEditor.Slide slide, string slideName, string roomName, GameObject slidePlane)
	{
        GameObject slideText = GameObject.Find ("/" + roomName + "/Slides/" + slideName + "/Text"); //BB 20210103
        if (slide.Text != null) {
			GameObject slideControlPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/SlideControl.prefab", typeof(GameObject)); //BB, 10.09.2019

			//GameObject slideText = GameObject.Find ("/" + roomName + "/Slides/" + slideName + "/Text");
			//slideText.GetComponent<TextMesh> ().text = slide.Text;
			SlideTextPortions slideBuffer = slideText.AddComponent<SlideTextPortions>(); //BB, 20190909
			slideBuffer.SetInputText(slide.Text);
			slideText.GetComponent<TextMesh> ().text = (string)slideBuffer.textSlidePortions [0];
			slideText.GetComponent<TextMesh> ().characterSize = 0.14f; //BB, 20171202, it was = 0.15f
			slideText.GetComponent<TextMesh> ().alignment = TextAlignment.Left;
			slideText.GetComponent<Renderer> ().enabled = true;
			//slideText.GetComponent<Renderer> ().isPartOfStaticBatch = true;
			/*for (short k = 0; k < slideBuffer.slidePortionsNo; k++) {
				Debug.Log ("Slide No "+k.ToString()+": "+slideBuffer.textSlidePortions[k]);
			}*/
			if (slideBuffer.slidePortionsNo > 1) { //we need to create a slide control set
				string textWithPageFooter = (string) slideBuffer.textSlidePortions[0] + "                                --- стр. 1 от " + (slideBuffer.slidePortionsNo).ToString() + " ---";
				slideText.GetComponent<TextMesh> ().text = textWithPageFooter;
				GameObject slideControl = createObject (slideControlPrefab, slideName + "Control");		//BB, 10.09.2019
				slideControl.transform.SetParent (slideText.transform.parent);							//BB, 10.09.2019
				slideControl.transform.localPosition = new Vector3 (0f, -0.45f, -0.025f);				//BB, 10.09.2019
				slideControl.transform.localRotation = Quaternion.Euler (90f, 0f, 0f);					//BB, 10.09.2019
				slideControl.transform.localScale = new Vector3  (0.15f, 0.15f, 0.15f);					//BB, 13.09.2019
			}
		}
		else { //BB
			//GameObject slideText = GameObject.Find ("/" + roomName + "/Slides/" + slideName + "/Text");
			slideText.GetComponent<TextMesh> ().text = "";
		}

        if (slide.Image != null) {
            if (slide.Image != "NONE")
            { //BB When slide.Image == "NONE" then no background will be benerated
                //Debug.Log ("slidePlane.GetComponent<MeshRenderer> ().sharedMaterials [1]: "+slidePlane.GetComponent<MeshRenderer> ().sharedMaterials[0]);
                //MeshFilter mf = slidePlane.GetComponent<MeshFilter> (); 
                //Mesh mesh = ((GameObject)Resources.Load ("Assets/_Prefabs/ThirdPartyPrefabs/_Creepy_Cat/_Museum/_Meshes/Paint_A.FBX")).GetComponent<MeshFilter>().mesh;
                //Mesh m = ((Mesh)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/ThirdPartyPrefabs/_Creepy_Cat/_Museum/_Meshes/Picture", typeof(Mesh)));
                //mf.mesh = m;
                Material tempMaterial = new Material(slidePlane.GetComponent<MeshRenderer>().sharedMaterials[1]);
                tempMaterial.mainTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Materials/Textures/" + slide.Image, typeof(Texture));
                Material[] tempMaterials = slidePlane.GetComponent<MeshRenderer>().sharedMaterials;
                //Material frameMaterial = new Material (slidePlane.GetComponent<MeshRenderer> ().sharedMaterials [0]);
                //tempMaterials [0] = frameMaterial;
                tempMaterials[1] = tempMaterial;
                if (showSlideFrames == "No")
                {
                    tempMaterials[0] = ((Material)AssetDatabase.LoadAssetAtPath("Assets/ProCore/ProBuilder/Resources/Materials/InvisibleFace.mat", typeof(Material))); //BB, 20171203, frames invisible!
                }
                slidePlane.GetComponent<MeshRenderer>().sharedMaterials = tempMaterials;
            }
            else
            { //BB, 20171202
                //slide.Image = "NONE" makes the MeshRenderer of that slide to be invisible
                MeshRenderer mr = slidePlane.GetComponent<MeshRenderer>();
                mr.enabled = false;
            }
        }
        if (slide.GameWordSoup != null) { //BB, 20210102
            Debug.Log("GameWordSoup.Points: " + slide.GameWordSoup.Points);
            int i = 0;
            GameObject dictionary = GameObject.Find("WordSoupDictionary");
            foreach (string row in slide.GameWordSoup.rows)
            {
                    //Debug.Log("Row: " + row);
                    int len = row.Length;
                    string currRow = row;
                    if (len < WIDTH)  {
                        for (int k = 0; k < WIDTH - len; k++) {
                            currRow += '#'; //we fill the row with # up to WIDTH characters
                        }
                    }
                    GameObject tilePrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/WordSoup/Tile01.prefab", typeof(GameObject)); //BB, 20210103
                    for (int j = 0; j < WIDTH; j++) {
                        GameObject tile = createObject(tilePrefab, "Tile" + System.Convert.ToChar(i + '0') + System.Convert.ToChar(j + '0'));
                        tile.transform.SetParent(slideText.transform.parent);
                        tile.transform.localPosition = new Vector3(0.429f - 0.125f * (float)j, 0.217f - 0.111f * (float)i, 0.024f);
                        tile.transform.localScale = new Vector3(0.1f, 0.1f, 0.03f);
                        string s = "";
                        s += currRow[j];
                        tile.transform.GetChild(0).GetComponent<TextMeshPro>().text = s;
                        tile.transform.GetChild(0).GetComponent<TextMeshPro>().enabled = false;
                    }
                    i += 1;
                    if (i == HEIGHT) break;
            }
            foreach (string word in slide.GameWordSoup.words)
                {
                    //Debug.Log("Word: " + word);
                    if (dictionary == null)
                    {
                        //we create a dictionary as an invisible object containing the words of the game dictionary
                        GameObject disctionaryPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/WordSoup/Dictionary.prefab", typeof(GameObject));
                        dictionary = createObject(disctionaryPrefab, "WordSoupDictionary");
                        dictionary.transform.SetParent(slideText.transform.parent);
                        dictionary.transform.localPosition = new Vector3(-0.95f, -0.3f, 10.0f);
                        dictionary.transform.GetChild(0).GetComponent<TextMeshPro>().text = word;
                        dictionary.transform.GetChild(1).GetComponent<TextMeshPro>().text = slide.GameWordSoup.Points;
                        dictionary.SetActive(false);
                    }
                    else
                    {
                        //GameWordSoup.dictionaryWordSoup.Add(word);
                        dictionary.transform.GetChild(0).GetComponent<TextMeshPro>().text += " " + word;
                    }
                }
            if (slide.GameWordSoup.Hints != null) {
                    //Debug.Log("Hints: " + slide.GameWordSoup.Hints);
                    GameObject hintsPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/WordSoup/PlaneHints.prefab", typeof(GameObject));
                    GameObject hints = createObject(hintsPrefab, "Hints");
                    hints.transform.SetParent(slideText.transform.parent);
                    hints.transform.localPosition = new Vector3(-0.95f, -0.3f, 0.0f);
                    hints.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                    hints.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    hints.transform.GetChild(0).GetComponent<TextMeshPro>().text = "\n\n"+slide.GameWordSoup.Hints;
                    hints.SetActive(false);
            }
		}

		if ( !(slide.Text == null || slide.Text == "") && !(slide.Image == defaultSlideBackground) && !(slide.Image == "NONE")) { //BB 20171202

			//Debug.Log ("defaultSlideBackground: " + defaultSlideBackground);
			//Debug.Log ("slide.Image: " + slide.Image);
			//Debug.Log ("!(slide.Image == defaultSlideBackground): " + !(slide.Image == defaultSlideBackground));

			//this slide has both text and image => we will add a box collider and a toggle script 
			//GameObject slideText = GameObject.Find ("/" + roomName + "/Slides/" + slideName + "/Text");
			//slideText.GetComponent<TextMesh> ().text = "slide.Text: "+slide.Text.ToString() +"slide.Image.ToString:" + slide.Image.ToString() +":";
			var boxCollider = slideText.AddComponent<BoxCollider>();
			boxCollider.size = new Vector3(12.0f, 8.0f, 2.0f);
			slideText.AddComponent<SlideTextOnOff>();
			slideText.GetComponent<Renderer> ().enabled = false; //slides with back image different than defaultSlideBackground or NONE will ot be shown intil the camera will arroach them
		}
	}

	private void setWallTexture(string wallName, string roomName, string roomWallTexture)
	{
		GameObject wall = GameObject.Find ("/" + roomName + "/Walls/" + wallName + "/DoorCover");
		Material tempMaterial = new Material(wall.GetComponent<MeshRenderer> ().sharedMaterial);
		tempMaterial.mainTexture = (Texture)AssetDatabase.LoadAssetAtPath ("Assets/Materials/Textures/" + roomWallTexture, typeof(Texture));
		wall.GetComponent<MeshRenderer> ().sharedMaterial = tempMaterial;
		wall = GameObject.Find ("/" + roomName + "/Walls/" + wallName + "/DoorEntrance");
		tempMaterial = new Material(wall.GetComponent<MeshRenderer> ().sharedMaterial);
		tempMaterial.mainTexture = (Texture)AssetDatabase.LoadAssetAtPath ("Assets/Materials/Textures/" + roomWallTexture, typeof(Texture));
		wall.GetComponent<MeshRenderer> ().sharedMaterial = tempMaterial;
		wall = GameObject.Find ("/" + roomName + "/Walls/" + wallName + "/WallSideLeft");
		tempMaterial = new Material(wall.GetComponent<MeshRenderer> ().sharedMaterial);
		tempMaterial.mainTexture = (Texture)AssetDatabase.LoadAssetAtPath ("Assets/Materials/Textures/" + roomWallTexture, typeof(Texture));
		wall.GetComponent<MeshRenderer> ().sharedMaterial = tempMaterial;
		wall = GameObject.Find ("/" + roomName + "/Walls/" + wallName + "/WallSideRight");
		tempMaterial = new Material(wall.GetComponent<MeshRenderer> ().sharedMaterial);
		tempMaterial.mainTexture = (Texture)AssetDatabase.LoadAssetAtPath ("Assets/Materials/Textures/" + roomWallTexture, typeof(Texture));
		wall.GetComponent<MeshRenderer> ().sharedMaterial = tempMaterial;
	}

	private void setStartingRoom(string roomName)
	{
		GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/CustomFPSController.prefab", typeof(GameObject));
		if (prefab != null) {
			GameObject fpsController = createObject (prefab, "CustomFPSController");
			GameObject room = GameObject.Find ("/" + roomName);
			fpsController.transform.position = new Vector3 (room.transform.position.x, room.transform.position.y, room.transform.position.z - 15);

		}
	}

	private void addGameManager(AssemblyCSharpEditor.Room xmlRoom)
	{
		string roomName = xmlRoom.Name;
		GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/GameManager.prefab", typeof(GameObject));
		if (prefab != null) {
			GameObject gameManager = createObject (prefab, "GameManager");
			GameObject room = GameObject.Find ("/" + roomName);
			gameManager.transform.SetParent (room.transform);
			gameManager.transform.position = new Vector3 (0, 0, 0);

			GameObject fpsController = GameObject.Find ("/CustomFPSController");
			gameManager.GetComponent<GameManager> ().fpsCharacter = fpsController;

			if (xmlRoom.AudioClip != null && xmlRoom.AudioClip.File != null && xmlRoom.AudioClip.File != "") {
				gameManager.GetComponent<AudioSource> ().clip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Materials/Sounds/" + xmlRoom.AudioClip.File, typeof(AudioClip));
				gameManager.GetComponent<AudioSource> ().loop = xmlRoom.AudioClip.Loop;
			}

		}
	}

	private void addHiddenObject(AssemblyCSharpEditor.Room xmlRoom)
	{
		string roomName = xmlRoom.Name;
		GameObject room = GameObject.Find ("/" + roomName);

		GameObject hiddenObjectParent = new GameObject ("HiddenObjects");
		hiddenObjectParent.transform.SetParent (room.transform);
		hiddenObjectParent.transform.position = new Vector3 (0, 0, 0);

		if (xmlRoom.hiddenObjects != null && xmlRoom.hiddenObjects.Count > 0) {

            float X_init = -12.0f; //BB, 20171208, initial X coordinate for the first hidden object placed within the room
            float X_offset = 1.5f; //BB, 20171208, step of offset for placing next hidden object
            int k = 0; //counter of hidden objects
			foreach(AssemblyCSharpEditor.HiddenObject currentHiddenObject in xmlRoom.hiddenObjects) {

				GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/HiddenObject.prefab", typeof(GameObject));
				if (prefab != null) {
					GameObject hiddenObject = createObject (prefab, "HiddenObject");

					hiddenObject.transform.SetParent (hiddenObjectParent.transform);
                    hiddenObject.transform.position = new Vector3(X_init + k * X_offset, -2.5f, 3.5f); //BB, 20171210, instead of new Vector3(0, 0, 0)

					if (currentHiddenObject.Texture != null && currentHiddenObject.Texture != "") {
						Material tempMaterial = new Material (hiddenObject.GetComponent<MeshRenderer> ().sharedMaterial);
						tempMaterial.mainTexture = (Texture)AssetDatabase.LoadAssetAtPath ("Assets/Materials/Textures/" + currentHiddenObject.Texture, typeof(Texture));
						Color tempColor = tempMaterial.color;
						tempColor.a = 0.25f;
						tempMaterial.color = tempColor;
						hiddenObject.GetComponent<MeshRenderer> ().sharedMaterial = tempMaterial;
					}

					if (currentHiddenObject.Points > 0) {
						//hiddenObject.SendMessage ("setPoints", currentHiddenObject.Points); //commented by BB on 09.09.2019  because of: 
						//Assertion failed: Assertion failed on expression: 'ShouldRunBehaviour()' 	UnityEngine.GameObject:SendMessage(String, Object)
					}
                    k++; //BB, 20171208
				}
			}
		}
	}

	private void addGlobalGameManager()
	{
		GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/GlobalGameManager.prefab", typeof(GameObject));
		if (prefab != null) {
			GameObject gameManager = createObject (prefab, "GlobalGameManager");
			gameManager.transform.position = new Vector3 (0, 0, 0);
		}
	}

    private void createGameRollABall(AssemblyCSharpEditor.Room room)
	{
		GameObject roomObject = GameObject.Find ("/" + room.Name);
        GameObject game = new GameObject("GameRollABall");
		game.transform.SetParent (roomObject.transform);
		game.transform.position = new Vector3 (0, 0, 0);

		GameObject ballPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/BallBoard.prefab", typeof(GameObject));
		GameObject circlePrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/CircleBoard.prefab", typeof(GameObject));
		GameObject ringPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/TorusBoard.prefab", typeof(GameObject));

		int ballCount = 0, ballRowCount = 1;
		int circleCount = 0, circleRowCount = 1;

        if (room.GameRollABall != null && room.GameRollABall.gameElements != null && room.GameRollABall.gameElements.Count > 0)
        {
            foreach (AssemblyCSharpEditor.GameElement gameElement in room.GameRollABall.gameElements)
            {
				int x;
				GameObject newElement = null;
				if (gameElement.Type == "Ball") {
					newElement = createObject (ballPrefab, (gameElement.Name != null && gameElement.Name != "" ? gameElement.Name : "Ball"));
					if (ballRowCount < 2) {
						x = 9;
					} else if (ballRowCount == 2) {
						x = 12;
					} else {
						x = 9 - 3 * (ballRowCount - 2);
					}
					newElement.transform.position = new Vector3 (roomObject.transform.position.x + x, 0, roomObject.transform.position.z - ballCount * 3);
					newElement.transform.SetParent (game.transform);
					ballCount++;
					if (ballCount == 6) {
						ballRowCount++;
						ballCount = 0;
					}

                    if (room.GameRollABall.MinPoints > 0)
                    {
                        //newElement.transform.FindChild ("Ball").gameObject.SendMessage ("setMinPoints", room.GameRollABall.MinPoints);
					}
				}

				if (newElement != null) {
					if (gameElement.Text != null && gameElement.Text != "") {
						newElement.transform.Find ("Board").Find ("Text").GetComponent<TextMesh> ().text = gameElement.Text;
						newElement.transform.Find ("Board").Find ("Text_Back").GetComponent<TextMesh> ().text = gameElement.Text;
					} else {
						newElement.transform.Find ("Board").Find ("Text").gameObject.SetActive (false);
						newElement.transform.Find ("Board").Find ("Text_Back").gameObject.SetActive (false);
					}

					GameObject plane = newElement.transform.Find ("Board").Find ("Plane").gameObject;
					GameObject plane_back = newElement.transform.Find ("Board").Find ("Plane_Back").gameObject;
					if (gameElement.Image != null && gameElement.Image != "") {
						Material tempMaterial = new Material (plane.GetComponent<MeshRenderer> ().sharedMaterial);
						tempMaterial.mainTexture = (Texture)AssetDatabase.LoadAssetAtPath ("Assets/Materials/Textures/" + gameElement.Image, typeof(Texture));
						plane.GetComponent<MeshRenderer> ().sharedMaterial = tempMaterial;
						plane_back.GetComponent<MeshRenderer> ().sharedMaterial = tempMaterial;
					} else {
						plane.SetActive (false);
						plane_back.SetActive (false);
						newElement.transform.Find ("Board").Find ("Text_Back").gameObject.SetActive (false); //BB, for not showing back text in case of no image, 05.09.2019
					}

					if (gameElement.Texture != null && gameElement.Texture != "") {
						GameObject actualElement = null;
						if (gameElement.Type == "Ball") {
							actualElement = newElement.transform.Find ("Ball").gameObject;
						}

						if (actualElement != null) {
							Material tempMaterial = new Material (actualElement.GetComponent<MeshRenderer> ().sharedMaterial);
							tempMaterial.mainTexture = (Texture)AssetDatabase.LoadAssetAtPath ("Assets/Materials/Textures/" + gameElement.Texture, typeof(Texture));
							actualElement.GetComponent<MeshRenderer> ().sharedMaterial = tempMaterial;	
						}
					}
				}
			}

            foreach (AssemblyCSharpEditor.GameElement gameElement in room.GameRollABall.gameElements)
            {
				int x;
				GameObject newElement = null;
				if (gameElement.Type == "Circle") {
					newElement = createObject (circlePrefab, (gameElement.Name != null &&  gameElement.Name != "" ? gameElement.Name : "Circle"));
					if (circleRowCount < 2) {
						x = -9;
					} else if (circleRowCount == 2) {
						x = -12;
					} else {
						x = -9 + 3 * (circleRowCount - 2);
					}
					newElement.transform.position = new Vector3(roomObject.transform.position.x + x, 0, roomObject.transform.position.z - circleCount*3);
					newElement.transform.SetParent (game.transform);
					circleCount++;
					if (circleCount == 6) {
						circleRowCount++;
						circleCount = 0;
					}

					if (gameElement.Ball != null && gameElement.Ball != "") {
                        GameObject ball = GameObject.Find("/" + room.Name + "/GameRollABall/" + gameElement.Ball + "/Ball");
						newElement.transform.Find("Circle").gameObject.GetComponent<MatchBall>().ball  = ball;
                        //Debug.Log(newElement.transform.Find("Circle").gameObject.name + " matches: " + ball.name);
					}
				}
				if (gameElement.Type == "Ring") {
					newElement = createObject (ringPrefab, (gameElement.Name != null &&  gameElement.Name != ""  ? gameElement.Name : "Ring"));
					newElement.transform.Find("Torus").GetComponent<BoxCollider> ().center = new Vector3 (-1, 0, 0);
					newElement.transform.Find("Torus").GetComponent<BoxCollider> ().size = new Vector3 (2, 0.2f, 0.52f);

					if (circleRowCount < 2) {
						x = -9;
					} else if (circleRowCount == 2) {
						x = -12;
					} else {
						x = -9 + 3 * (circleRowCount - 2);
					}
					newElement.transform.position = new Vector3(roomObject.transform.position.x + x, 0, roomObject.transform.position.z - circleCount*3);
					newElement.transform.SetParent (game.transform);
					circleCount++;
					if (circleCount == 6) {
						circleRowCount++;
						circleCount = 0;
					}

					if (gameElement.Ball != null && gameElement.Ball != "") {
                        GameObject ball = GameObject.Find("/" + room.Name + "/GameRollABall/" + gameElement.Ball + "/Ball");
						newElement.transform.Find("Torus").gameObject.GetComponent<TorusPassThrough>().ball  = ball;
					}
				}

				if (newElement != null) {
					if (gameElement.Text != null && gameElement.Text != "") {
						newElement.transform.Find ("Board").Find ("Text").GetComponent<TextMesh> ().text = gameElement.Text;
						newElement.transform.Find ("Board").Find ("Text_Back").GetComponent<TextMesh> ().text = gameElement.Text;
					} else {
						newElement.transform.Find ("Board").Find ("Text").gameObject.SetActive (false);
						newElement.transform.Find ("Board").Find ("Text_Back").gameObject.SetActive (false);
					}

					GameObject plane = newElement.transform.Find ("Board").Find ("Plane").gameObject;
					GameObject plane_back = newElement.transform.Find ("Board").Find ("Plane_Back").gameObject;
					if (gameElement.Image != null && gameElement.Image != "") {
						Material tempMaterial = new Material (plane.GetComponent<MeshRenderer> ().sharedMaterial);
						tempMaterial.mainTexture = (Texture)AssetDatabase.LoadAssetAtPath ("Assets/Materials/Textures/" + gameElement.Image, typeof(Texture));
						plane.GetComponent<MeshRenderer> ().sharedMaterial = tempMaterial;
						plane_back.GetComponent<MeshRenderer> ().sharedMaterial = tempMaterial;	
					} else {
						plane.SetActive (false);
						plane_back.SetActive (false);
						newElement.transform.Find ("Board").Find ("Text_Back").gameObject.SetActive (false); //BB, for not showing back text in case of no image, 05.09.2019
					}

					if (gameElement.Texture != null && gameElement.Texture != "") {
						GameObject actualElement = null;
						if (gameElement.Type == "Ball") {
							actualElement = newElement.transform.Find ("Ball").gameObject;
						}

						if (gameElement.Type == "Circle") {
							actualElement = newElement.transform.Find ("Circle").gameObject;
						}

						if (gameElement.Type == "Ring") {
							actualElement = newElement.transform.Find ("Torus").gameObject;
						}

						if (actualElement != null) {
							Material tempMaterial = new Material (actualElement.GetComponent<MeshRenderer> ().sharedMaterial);
							tempMaterial.mainTexture = (Texture)AssetDatabase.LoadAssetAtPath ("Assets/Materials/Textures/" + gameElement.Texture, typeof(Texture));
							actualElement.GetComponent<MeshRenderer> ().sharedMaterial = tempMaterial;	
						}
					}
				}
			}


		}
	}

}