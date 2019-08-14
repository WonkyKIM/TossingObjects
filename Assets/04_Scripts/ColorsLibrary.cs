using UnityEngine;

public class ColorsLibrary : MonoBehaviour {

    public Color White = new Color(1,1,1,1);
    public Color Grey = new Color(0.4f,0.4f,0.4f,1);
    public Color Yellow = new Color(255f/255f, 190f/255f, 0, 1);
    public Color Orange = new Color(255f/255f, 100f/255f, 0, 1);
    public Color Green = new Color(35f/255f, 230f/255f, 110f/255f, 1);
    public Color Red = new Color(230f/255f, 65f/255f, 35f/255f, 1);

    public static ColorsLibrary inst;

    private void Awake() {
        if(inst == null) {
            inst = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            if(inst != this) {
                Destroy(this.gameObject);
            }
        }
    }

}
