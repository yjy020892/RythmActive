using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(NoteCreateManager))]
public class SongControlButton : Editor
{
    bool posGroupEnabled = true;
    bool[] pos = new bool[] { false, false };

    string str = string.Empty;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NoteCreateManager createrManager = (NoteCreateManager)target;

        EditorGUILayout.Space();

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Start Song", GUILayout.MaxWidth(150)))
            {
                createrManager.StartSong();
            }

            if (GUILayout.Button("Pause Song", GUILayout.MaxWidth(150)))
            {
                createrManager.PauseSong();
            }

            if (GUILayout.Button("Stop Song", GUILayout.MaxWidth(150)))
            {
                createrManager.StopSong();
            }
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.HelpBox("노래시작", MessageType.None);
            EditorGUILayout.HelpBox("일시정지", MessageType.None);
            EditorGUILayout.HelpBox("노래정지", MessageType.None);
        }

        EditorGUILayout.Space();
        //createrManager.BPM = EditorGUILayout.IntField("Int Field", createrManager.BPM);

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Set BPM"))
            {
                createrManager.SetBPM();
            }

            if (GUILayout.Button("+ Beat"))
            {
                createrManager.PlusBeat();
            }

            if (GUILayout.Button("- Beat"))
            {
                createrManager.MinusBeat();
            }
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.HelpBox("BPM적용", MessageType.None);
            EditorGUILayout.HelpBox("박자+", MessageType.None);
            EditorGUILayout.HelpBox("박자-", MessageType.None);
        }

        EditorGUILayout.Space();

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Set MusicTime", GUILayout.MaxWidth(130)))
            {
                createrManager.SetMusicTime();
            }

            str = EditorGUILayout.TextField(str, GUILayout.MaxWidth(40));

            if (!string.IsNullOrEmpty(str))
            {
                createrManager.changeMusicTime = int.Parse(str);
            }
            else
            {
                createrManager.changeMusicTime = 0;
            }

            if (GUILayout.Button("+", GUILayout.MaxWidth(20)))
            {
                createrManager.PlusMusicTime();
            }

            if (GUILayout.Button("-", GUILayout.MaxWidth(20)))
            {
                createrManager.MinusMusicTime();
            }

            if (GUILayout.Button("SubBPM", GUILayout.MaxWidth(80)))
            {
                createrManager.SetSubBPM();
            }
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.HelpBox("MusicTime적용", MessageType.None);
            EditorGUILayout.HelpBox("시간+", MessageType.None);
            EditorGUILayout.HelpBox("시간-", MessageType.None);
        }

        EditorGUILayout.Space();

        using (new EditorGUILayout.HorizontalScope())
        {
            createrManager.b_Click = EditorGUILayout.ToggleLeft("클릭", createrManager.b_Click, GUILayout.MaxWidth(60));

            createrManager.b_Dance = EditorGUILayout.ToggleLeft("댄스", createrManager.b_Dance, GUILayout.MaxWidth(60));

            createrManager.b_Self = EditorGUILayout.ToggleLeft("셀프", createrManager.b_Self, GUILayout.MaxWidth(80));
        }

        EditorGUILayout.Space();

        //posGroupEnabled = EditorGUILayout.BeginToggleGroup("Align position", posGroupEnabled);
        using (new EditorGUILayout.HorizontalScope())
        {
            createrManager.b_LeftHand = EditorGUILayout.ToggleLeft("왼손", createrManager.b_LeftHand, GUILayout.MaxWidth(60));

            createrManager.b_RightHand = EditorGUILayout.ToggleLeft("오른손", createrManager.b_RightHand, GUILayout.MaxWidth(80));

            createrManager.b_XMirror = EditorGUILayout.ToggleLeft("X Mirror", createrManager.b_XMirror, GUILayout.MaxWidth(80));

            createrManager.b_YMirror = EditorGUILayout.ToggleLeft("Y Mirror", createrManager.b_YMirror, GUILayout.MaxWidth(80));
        }

        EditorGUILayout.Space();

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Save", GUILayout.MaxWidth(150)))
            {
                createrManager.SaveList();
            }

            if (GUILayout.Button("Reset", GUILayout.MaxWidth(150)))
            {
                createrManager.ClearList();
            }

            if (GUILayout.Button("Remove", GUILayout.MaxWidth(150)))
            {
                createrManager.RemoveList();
            }
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.HelpBox("리스트저장", MessageType.None);
            EditorGUILayout.HelpBox("리스트초기화", MessageType.None);
            EditorGUILayout.HelpBox("리스트삭제", MessageType.None);
        }
        //EditorGUILayout.EndToggleGroup();
    }
}
#endif