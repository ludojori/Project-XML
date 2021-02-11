using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

namespace AssemblyCSharpEditor
{
	[XmlRoot("Labyrinth")]	
	public class Labyrinth
	{
        [XmlElement("GlobalSettings")] //BB, 20171207
        public GlobalSettings GlobalSettings { get; set; }

        [XmlArray("Rooms")]
        [XmlArrayItem("Room")]
        public List<Room> Rooms = new List<Room>();



		public static Labyrinth LoadXml(string path)
		{
			XmlSerializer serializer = new XmlSerializer (typeof(Labyrinth));
			using (FileStream stream = new FileStream (path, FileMode.Open)) {
				return serializer.Deserialize (stream) as Labyrinth;
			}
		}
	}

    public class GlobalSettings //BB, 20171207
    {
        [XmlElement("Language")]
        public string Language;
        [XmlElement("DefaultSlideBackground")]
        public string DefaultSlideBackground;
        [XmlElement("Illumination")]
        public string Illumination;
        [XmlElement("ShowDoorLock")]
        public string ShowDoorLock;
        [XmlElement("ShowSlideFrames")]
        public string ShowSlideFrames;
        [XmlElement("ShowPlants")]
        public string ShowPlants;
        [XmlElement("CeilingTiling")]
        public float CeilingTiling;
        [XmlElement("FloorTiling")]
        public float FloorTiling;
    }

	public class Room
	{
		public string Name;

		public Audio AudioClip;

		public Door DoorN;
		public Door DoorE;
		public Door DoorW;
		public Door DoorS;

		public Slide SlideN_1;
		public Slide SlideN_2;
		public Slide SlideE_1;
		public Slide SlideE_2;
		public Slide SlideW_1;
		public Slide SlideW_2;
		public Slide SlideS_1;
		public Slide SlideS_2;

		public string WallTexture;
		public string FloorTexture;
		public string CeilingTexture;
		public string Map;

		[XmlArray("HiddenObjects")]
		[XmlArrayItem("HiddenObject")]
		public List<HiddenObject> hiddenObjects = new List<HiddenObject>();

		public bool StartingRoom;

        public GameRollABall GameRollABall;
        public GameOtherType GameOtherType;
    }

    public class GameRollABall
	{
		public int MinPoints;
		[XmlArray("GameElements")]
		[XmlArrayItem("GameElement")]
		public List<GameElement> gameElements = new List<GameElement>();
	}

    public class GameElement
    {
        public string Text;
        public string Image;
        public string Texture;
        public string Name;
        public string Type;
        public string Ball;
    }

    public class GameWordSoup
    {
        [XmlAttribute("Points")]
        public string Points;
        [XmlArray("Rows")]
        [XmlArrayItem("Row")]
        public List<string> rows = new List<string>();
        [XmlArray("Dictionary")]
        [XmlArrayItem("Word")]
        public List<string> words = new List<string>();
        //[XmlArray("Hints")]
        //[XmlArrayItem("Hint")]
        //public List<string> hints = new List<string>();
        [XmlElement("Hints")]
        public string Hints;
    }

    public class GameOtherType
    {
        //this is a sample structure of the settings of GameOtherType:
        [XmlAttribute("AttrName")]
        public string AttrName;
        [XmlArray("Settings")]
        [XmlArrayItem("Setting")]
        public List<string> rows = new List<string>();
        [XmlArray("Dictionary")]
        [XmlArrayItem("Word")]
        public List<string> words = new List<string>();
        [XmlArray("Hints")]
        [XmlArrayItem("Hint")]
        public List<string> hints = new List<string>();
    }
    
    public class Audio
	{
		public bool Loop;
		public string File;
	}

	public class HiddenObject
	{
		public string Texture;
		public int Points;
	}

	public class Slide
	{
		public string Text;
        public string Image;
        public GameWordSoup GameWordSoup; //BB - 20210102
        //public Game2DPuzzle GameWordSoup; //BB - 20210102
    }

	public class Question
	{
		public string Text;
		public string Image;
		public string Answer;
	}

	public class Door {
		public string Room;
		public Question Question;
	}
}