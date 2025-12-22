using UnityEngine.Events;

public interface IClickable
{
    public void AddClickHandler(UnityAction handleClick);
}