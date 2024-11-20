using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InGameSkill))]
public class InGameSkillEditor : Editor
{
    SerializedProperty skillName;
    SerializedProperty skillDescription;
    SerializedProperty skillIcon;
    SerializedProperty skillType;

    SerializedProperty skillObject;
    SerializedProperty stage1Effect;
    SerializedProperty stage2Effect;
    SerializedProperty stage3Effect;
    SerializedProperty stage4Effect;

    SerializedProperty spawnObject;
    SerializedProperty spawnInterval;

    SerializedProperty particlePrefab;
    SerializedProperty particleSizes;
    SerializedProperty particleInterval;

    private void OnEnable()
    {
        skillName = serializedObject.FindProperty("skillName");
        skillDescription = serializedObject.FindProperty("skillDescription");
        skillIcon = serializedObject.FindProperty("skillIcon");
        skillType = serializedObject.FindProperty("skillType");

        skillObject = serializedObject.FindProperty("skillObject");
        stage1Effect = serializedObject.FindProperty("stage1Effect");
        stage2Effect = serializedObject.FindProperty("stage2Effect");
        stage3Effect = serializedObject.FindProperty("stage3Effect");
        stage4Effect = serializedObject.FindProperty("stage4Effect");

        spawnObject = serializedObject.FindProperty("spawnObject");
        spawnInterval = serializedObject.FindProperty("spawnInterval");

        particlePrefab = serializedObject.FindProperty("particlePrefab");
        particleSizes = serializedObject.FindProperty("particleSizes");
        particleInterval = serializedObject.FindProperty("particleInterval");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(skillName);
        EditorGUILayout.PropertyField(skillDescription);
        EditorGUILayout.PropertyField(skillIcon);
        EditorGUILayout.PropertyField(skillType);

        InGameSkill.SkillType selectedType = (InGameSkill.SkillType)skillType.enumValueIndex;

        EditorGUILayout.Space();
        switch (selectedType)
        {
            case InGameSkill.SkillType.StageEffect:
                DrawStageEffectSettings();
                break;
            case InGameSkill.SkillType.SpawnEffect:
                DrawSpawnEffectSettings();
                break;
            case InGameSkill.SkillType.ParticleEffect:
                DrawParticleEffectSettings();
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawStageEffectSettings()
    {
        EditorGUILayout.LabelField("Stage Effect Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(skillObject);
        EditorGUILayout.PropertyField(stage1Effect);
        EditorGUILayout.PropertyField(stage2Effect);
        EditorGUILayout.PropertyField(stage3Effect);
        EditorGUILayout.PropertyField(stage4Effect);
    }

    private void DrawSpawnEffectSettings()
    {
        EditorGUILayout.LabelField("Spawn Effect Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(spawnObject);
        EditorGUILayout.PropertyField(spawnInterval);
    }

    private void DrawParticleEffectSettings()
    {
        EditorGUILayout.LabelField("Particle Effect Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(particlePrefab);
        EditorGUILayout.PropertyField(particleSizes, true);
        EditorGUILayout.PropertyField(particleInterval);
    }
}
