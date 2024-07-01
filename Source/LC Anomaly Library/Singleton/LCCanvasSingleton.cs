using LCAnomalyLibrary.GameComponent;
using UnityEngine;
using UnityEngine.UI;
using Verse;
using static LCAnomalyLibrary.Util.MusicUtils;

namespace LCAnomalyLibrary.Singleton
{

    public class LCCanvasSingleton
    {
        public static LCCanvasSingleton Instance
        {
            get
            {
                if (instance == null)
                    instance = new LCCanvasSingleton();
                return instance;
            }
        }
        private static LCCanvasSingleton instance = null;

        public GameObject GameObject;
        public Canvas Canvas;
        public Image Image;
        public CanvasGroup CanvasGroup;

        public Sprite FirstWarningSprite = Sprite.Create(ContentFinder<Texture2D>.Get("UI/WarningUI/WarningUI_First"), new Rect(0, 0, 4096, 2160), new Vector2(0.5f, 0.5f));
        public Sprite SecondWarningSprite = Sprite.Create(ContentFinder<Texture2D>.Get("UI/WarningUI/WarningUI_Second"), new Rect(0, 0, 4096, 2160), new Vector2(0.5f, 0.5f));
        public Sprite ThirdWarningSprite = Sprite.Create(ContentFinder<Texture2D>.Get("UI/WarningUI/WarningUI_Third"), new Rect(0, 0, 4096, 2160), new Vector2(0.5f, 0.5f));

        private int tickCounter;
        private bool shouldShowWarningUI;
        private bool shouldDecreaseAlpha;

        public LCCanvasSingleton() 
        {
            this.GameObject = new GameObject();
            this.GameObject.name = "LCAnomalyCanvas";

            this.Canvas = this.GameObject.AddComponent<Canvas>();
            this.Canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            this.Image = this.GameObject.AddComponent<Image>();
            this.Image.sprite = FirstWarningSprite;
            this.Image.material.color = Color.red;

            this.CanvasGroup = this.GameObject.AddComponent<CanvasGroup>();
            this.CanvasGroup.interactable = false;
            this.CanvasGroup.blocksRaycasts = false;
            this.CanvasGroup.ignoreParentGroups = false;
            this.CanvasGroup.alpha = 1f;
            
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            Current.Game.GetComponent<GameComponent_LC>().LCGameComponentTickEvent += Tick;
        }

        public void RemoveEvents()
        {
            Current.Game.GetComponent<GameComponent_LC>().LCGameComponentTickEvent -= Tick;
        }

        protected void Tick()
        {
            if (shouldShowWarningUI)
            {
                if (shouldDecreaseAlpha)
                {
                    this.CanvasGroup.alpha -= 0.01f;
                }
                else
                {
                    this.CanvasGroup.alpha += 0.01f;
                }

                if(this.CanvasGroup.alpha >= 1f)
                {
                    shouldDecreaseAlpha = true;
                }
                else if(this.CanvasGroup.alpha <= 0f)
                {
                    shouldDecreaseAlpha = false;
                }

            }

            tickCounter++;
            //30s关闭
            if (tickCounter >= 1800)
            {
                shouldShowWarningUI = false;
                this.CanvasGroup.alpha = 0f;
                tickCounter = 0;
            }
        }


        public void ShowWarningUI(EWarningLevel level)
        {
            if(level == EWarningLevel.First)
            {
                this.Image.sprite = FirstWarningSprite;
                this.Image.material.color = Color.yellow;
            }
            else if (level == EWarningLevel.Second)
            {
                this.Image.sprite = SecondWarningSprite;
                this.Image.material.color = Color.red + Color.yellow;
            }
            else if (level == EWarningLevel.Third)
            {
                this.Image.sprite = ThirdWarningSprite;
                this.Image.material.color = Color.red;
            }

            tickCounter = 0;
            shouldShowWarningUI = true;
        }
    }
}
