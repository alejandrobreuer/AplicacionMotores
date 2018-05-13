using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; //necesario importar para trabajar en editor
using UnityEditor.AnimatedValues; //para usar los animated values
using System;

/*
LINKS DE AYUDA:
https://docs.unity3d.com/Manual/editor-CustomEditors.html
https://docs.unity3d.com/ScriptReference/EditorGUI.html
https://docs.unity3d.com/ScriptReference/GUI.html
https://docs.unity3d.com/ScriptReference/EditorGUILayout.html
*/

[CustomEditor(typeof(Hero))]
public class HeroEditor : Editor
{
    private Hero _hero;
    private GUIStyle _labelStyle;
    private int _currentClassElection;
    private Vector2 _scrollPosition;
    private bool _showFoldout;
    private AnimBool _enableMore;
    private bool myBool;

    //se ejecuta al aparecer. En este caso, cuando seleccionamos el objeto en escena.
    private void OnEnable()
    {
        //guardamos la referencia al objeto seleccionado. Como "target" es un objeto, hay que castearlo.
        _hero = (Hero)target;

        //creo un estilo propio y lo guardo
        _labelStyle = new GUIStyle();
        _labelStyle.fontStyle = FontStyle.Italic;
        _labelStyle.alignment = TextAnchor.MiddleCenter;

        _enableMore = new AnimBool(false); // Creamos el anim bool y le pasamos por parámetro su valor inicial
        _enableMore.valueChanged.AddListener(Repaint); // Le agregamos un listener para que cuando cambie su valor haga el fade
    }

    //Aca se dibuja el inspector. Todo lo que hagamos sale de acá
    public override void OnInspectorGUI()
    {
        //podemos bloquear la edicion dependiendo si estamos en playmode o no
        if(Application.isPlaying)
        {
            EditorGUILayout.HelpBox("NO SE PUEDE EDITAR PARAMETROS DURANTE PLAY MODE", MessageType.Warning);
            return;
        }

        //dibuja el inspector "por default". útil si queremos solamente agregar algo a lo que ya está.
        //DrawDefaultInspector();

        DrawTextAndTextures();

        //permite poner espacios en medio del editor
        EditorGUILayout.Space();

        //permite dibujar líneas o cuadrados.
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);

        EditorGUILayout.Space();

        DrawHeroParameters();

        //nos permite editar el espacio de preview que aparece abajo
        //OnPreviewGUI(GUILayoutUtility.GetRect(100, 100), /*ACA TENGO QUE PASARLE UN GUISTYLE PROPIO PARA EL BACKGROUND*/);
    }

    private void DrawTextAndTextures()
    {
        //para agregar textos personalizados
        EditorGUILayout.LabelField("UN LABEL");

        //Usando los GUISTyles
        EditorGUILayout.LabelField("Un label con estilo", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Un label con estilo propio", _labelStyle);

        //Mostramos imagenes personalizadas, sean texturas o sprites
        GUI.DrawTexture(GUILayoutUtility.GetRect(335, 250), (Texture2D)Resources.Load("flag"), ScaleMode.ScaleToFit);

        //dibujar el alpha de una imagen, o con el fondo transparente
        EditorGUI.DrawTextureAlpha(GUILayoutUtility.GetRect(175, 175), (Texture2D)Resources.Load("flag"), ScaleMode.ScaleToFit);
        EditorGUI.DrawTextureTransparent(GUILayoutUtility.GetRect(175, 175), (Texture2D)Resources.Load("flag"), ScaleMode.ScaleToFit);
    }

    private void DrawHeroParameters()
    {
        //para modificar strings
        _hero.myName = EditorGUILayout.TextField("My name", _hero.myName);

        //Modificamos un int
        _hero.healthPoints = EditorGUILayout.IntField("HP", _hero.healthPoints);

        //chequeamos valores inválidos
        if(_hero.healthPoints < 0)
        {
            //si es inválido, cortamos la funcion, lo acomodamos y repintamos el inspector.
            _hero.healthPoints = 0;
            Repaint();
            return;
        }

        //Modificamos un float
        _hero.speed = EditorGUILayout.FloatField("SPD", _hero.speed);

        //Modificamos un vector3
        _hero.respawnPosition = EditorGUILayout.Vector3Field("Respawn Position", _hero.respawnPosition);

        //modificamos un booleano
        myBool = EditorGUILayout.Toggle("un booleano", myBool);

        //Podemos agregar otros tipos, como en este caso, un Hero
        _hero.theOtherHero = (Hero)EditorGUILayout.ObjectField("The Other Hero", _hero.theOtherHero, typeof(Hero), true);

        //Modificamos un color
        //guardo el color previo para comparación
        var prevColor = _hero.playerColor;
        _hero.playerColor = EditorGUILayout.ColorField("Player Color", _hero.playerColor);

        //si hubo un cambio corto acá y redibujo el inspector, para evitar errores.
        if ((prevColor.a != 0 && _hero.playerColor.a == 0) || (prevColor.a == 0 && _hero.playerColor.a != 0))
        {
            Repaint();
            return;
        }

		//con IFs podemos controlar la aparicion o desaparición de elementos
        if (_hero.playerColor.a == 0)
        {
            //Agregamos advertencias
            EditorGUILayout.HelpBox("Los colores vienen con el Alpha en 0 por defecto", MessageType.Info);
            EditorGUILayout.HelpBox("Se pueden cambiar los íconos de los mensajes", MessageType.Warning);
            EditorGUILayout.HelpBox("Pero son solo efectos visuales", MessageType.Error);
        }

        //Podemos crear sliders
        _hero.ammountOfExperience = EditorGUILayout.Slider(_hero.ammountOfExperience, 0, 100);

        //Mostrar esa (u otra) información en forma de barra
        EditorGUI.ProgressBar(GUILayoutUtility.GetRect(15, 15), _hero.ammountOfExperience / 100, "" + _hero.ammountOfExperience + "%");

        //Podemos mostrar opciones en un popup
        // EditorGUILayout.Popup devuelve un número con lo cual vamos a tener que filtrar por el mismo
        _currentClassElection = EditorGUILayout.Popup("Hero Class", _currentClassElection, _hero.heroClasses);
        _hero.heroClass = _hero.heroClasses[_currentClassElection];

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //se pueden agregar parámetros extra a los elementos, que permiten controlar el ancho y alto del mismo
        //GUILayout.Height(), GUILayout.Width(), GUILayout.ExpandHeight(), GUILayout.ExpandWidth, GUILayout.MaxWidth(), GUILayout.MinWidth(), GUILayout.MaxHeight(), GUILayout.MinHeight() 
        _hero.optionsNumber = EditorGUILayout.IntField(_hero.optionsNumber, GUILayout.Height(50), GUILayout.Width(100), GUILayout.ExpandWidth(false));

        EditorGUILayout.Space();
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
        EditorGUILayout.Space();

        //podemos crear grupos horizontales o verticales
        //siempre entremedio de un "begin..." y un "end..."
        EditorGUILayout.BeginHorizontal();
        GUI.DrawTexture(GUILayoutUtility.GetRect(50, 50), (Texture2D)Resources.Load("flag"), ScaleMode.ScaleToFit);
        GUI.DrawTexture(GUILayoutUtility.GetRect(50, 50), (Texture2D)Resources.Load("flag"), ScaleMode.ScaleToFit);
        GUI.DrawTexture(GUILayoutUtility.GetRect(50, 50), (Texture2D)Resources.Load("flag"), ScaleMode.ScaleToFit);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
        EditorGUILayout.Space();

        //y podemos mezclarlos!!!
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Opcion1", GUILayout.ExpandWidth(false), GUILayout.Width(100));
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Toggle(false, GUILayout.Width(10), GUILayout.ExpandWidth(false));
        EditorGUILayout.LabelField("bueno", GUILayout.Width(40), GUILayout.ExpandWidth(false));
        EditorGUILayout.EndVertical();
        EditorGUILayout.LabelField("Opcion2", GUILayout.ExpandWidth(false), GUILayout.Width(100));
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Toggle(false, GUILayout.Width(10), GUILayout.ExpandWidth(false));
        EditorGUILayout.LabelField("malo", GUILayout.Width(40), GUILayout.ExpandWidth(false));
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        //FIJENSE QUE AUNQUE USE LOS BOOLEANOS ESOS, NO VA A PASAR NADA, PORQUE NO ESTOY GUARDANDO EL RESULTADO EN NINGUNA VARIABLE!!


        EditorGUILayout.Space();
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
        EditorGUILayout.Space();

        //Además podemos agregar scrollbars
        EditorGUILayout.BeginVertical(GUILayout.Height(110));
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, true, true);
        GUILayout.Label("Texto E es el más largo de todos\n los\n textos\n tan\n largo\n que\n sobresale\n y necesita un scrollbar para verse bien porque es muy largo. Quizá demasiado largo\nY\agrego\nsaltos\nde\nlinea\npara\nque\nse\nnote\nel\nscroll");
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //Podemos ocultar o mostrar información con los foldouts
        _showFoldout = EditorGUILayout.Foldout(_showFoldout, "Is showing: " + _showFoldout);
        if (_showFoldout)
            EditorGUILayout.HelpBox("Se esta mostrando el contenido", MessageType.None);

        //Podemos crear fade groups
        _enableMore.target = EditorGUILayout.Toggle("Habilitar", _enableMore.target);
        if (EditorGUILayout.BeginFadeGroup(_enableMore.faded))
        {
            _hero.myName = EditorGUILayout.TextField("Name", _hero.myName);
        }
        EditorGUILayout.EndFadeGroup();
    }
}
