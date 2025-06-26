using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(TargetsSet))]
public class TargetSetPropertyDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement result = new VisualElement();
        result.style.flexDirection = FlexDirection.Row;
        result.style.justifyContent = Justify.SpaceBetween;

        TextElement label = new TextElement();
        label.text = property.displayName;
        label.style.width = 75;
        label.style.marginLeft = 4;
        result.Add(label);

        PropertyField id1 = new PropertyField(property.FindPropertyRelative("_id1"));
        id1.label = "";
        id1.style.flexGrow = 2;
        result.Add(id1);

        PropertyField id2 = new PropertyField(property.FindPropertyRelative("_id2"));
        id2.label = "";
        id2.style.flexGrow = 2;
        result.Add(id2);

        PropertyField id3 = new PropertyField(property.FindPropertyRelative("_id3"));
        id3.label = "";
        id3.style.flexGrow = 2;
        result.Add(id3);

        PropertyField id4 = new PropertyField(property.FindPropertyRelative("_id4"));
        id4.label = "";
        id4.style.flexGrow = 2;
        result.Add(id4);

        PropertyField id5 = new PropertyField(property.FindPropertyRelative("_id5"));
        id5.label = "";
        id5.style.flexGrow = 2;
        result.Add(id5);

        PropertyField id6 = new PropertyField(property.FindPropertyRelative("_id6"));
        id6.label = "";
        id6.style.flexGrow = 2;
        result.Add(id6);

        PropertyField id7 = new PropertyField(property.FindPropertyRelative("_id7"));
        id7.label = "";
        id7.style.flexGrow = 2;
        result.Add(id7);

        PropertyField id8 = new PropertyField(property.FindPropertyRelative("_id8"));
        id8.label = "";
        id8.style.flexGrow = 2;
        result.Add(id8);

        return result;
    }
}