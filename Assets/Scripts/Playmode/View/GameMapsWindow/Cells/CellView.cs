using Playmode.Installers;
using Playmode.PlayData.ClientsData;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Playmode.View
{
    public abstract class CellView : MonoBehaviour
    {
        [SerializeField] Image _icon;
        [SerializeField] Image _BGImage;

        public Image Icon { get => _icon; set => _icon = value; }
        public Image BGImage { get => _BGImage; set => _BGImage = value; }
        public RectTransform RectTransform => _transform;

        protected ClientsCellData cellData;

        private RectTransform _transform;
        
        protected void Start()
        {
            AdaptateToPosition();
        }

        public virtual void SetCellData(ClientsCellData cellData)
        {
            this.cellData = cellData;

            _transform = GetComponent<RectTransform>();

        }

        protected virtual void AdaptateToPosition()
        {
            /*            var distance = 5 * _transform.rect.width;
                        if (transform.localPosition.y > distance)
                        {
                            cellPosition = CellDirection.Top;
                        }
                        else if (transform.localPosition.y < -distance)
                        {
                            cellPosition = CellDirection.Down;
                            transform.rotation = Quaternion.Euler(0, 0, -180);
                            Icon.transform.localRotation = Quaternion.Euler(0, 0, 180);
                        }
                        else if (transform.localPosition.x > distance)
                        {
                            cellPosition = CellDirection.Right;
                            transform.rotation = Quaternion.Euler(0, 0, -90);
                        }
                        else
                        {
                            cellPosition = CellDirection.Left;
                            transform.rotation = Quaternion.Euler(0, 0, 90);
                        }*/

            if (cellData.Direction == CellDirection.Top)
            {
            }
            else if (cellData.Direction == CellDirection.Down)
            {
                transform.localRotation = Quaternion.Euler(new(0, 0, -180));
                Icon.transform.localRotation = Quaternion.Euler(new(0, 0, -90));
            }
            else if (cellData.Direction == CellDirection.Right)
            {
                transform.localRotation = Quaternion.Euler(new(0, 0, -90));
            }
            else
            {
                transform.localRotation = Quaternion.Euler(new(0, 0, 90));
                Icon.transform.localRotation = Quaternion.Euler(new(0, 0, -90));
            }
        }
    }
}