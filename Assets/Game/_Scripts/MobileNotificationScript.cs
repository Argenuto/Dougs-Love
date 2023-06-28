using NotificationSamples;
using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class NotificationObject {
    public const string defaultChannelId = "game_channel0";
    public string title, body;
    [HideInInspector]
    public string smallIcon,largeIcon;
    public int badgeNumber;
    public float minutesToShow=0.15f;
    public bool reschedule;
    public GameNotificationsManager MyGNM { get; set; }
    public string ChannelId { get; set; }

    public void SendNotification() {
        IGameNotification notification = MyGNM.CreateNotification();
        if (notification == null)
        {
            return;
        }
        notification.Title = title;
        notification.Body = body;
        notification.Group = !string.IsNullOrEmpty(ChannelId) ? ChannelId : defaultChannelId;
        notification.DeliveryTime = DateTime.Now.AddMinutes(minutesToShow);
        notification.SmallIcon = smallIcon;
        notification.LargeIcon = largeIcon;
        if (badgeNumber != 0)
        {
            notification.BadgeNumber = badgeNumber;
        }

        PendingNotification notificationToDisplay = MyGNM.ScheduleNotification(notification);
        notificationToDisplay.Reschedule = reschedule;
        Debug.Log("notificacion pendiente en cola para "+notification.DeliveryTime.ToString());
    }

}
[Serializable]
public class MobileNotificationScript : MonoBehaviour
{
    public const string DLChannel = "DougsLoveChannel";
    public List<NotificationObject> notificationsObjects,spanishNoticationsObjects,japaneseNotificationObjects;
    GameNotificationsManager manager;

    // Start is called before the first frame update
    private void Awake()
    {
        manager = FindObjectOfType<GameNotificationsManager>();
        manager.LocalNotificationDelivered += OnDelivered;
        manager.LocalNotificationExpired += OnExpired;
        var dougsChannel = new GameNotificationChannel(DLChannel, "Doug Love Channel", "Doug's love notification Channel");
        manager.Initialize(dougsChannel);
    }

    private void OnExpired(PendingNotification obj)
    {
        Debug.Log("notificacion cancelada");
    }

    private void OnDelivered(PendingNotification obj)
    {
        Debug.Log("Notificación enviada");
        Debug.Log(obj.Notification.DeliveryTime);
    }

    void Start()
    {

        List<NotificationObject> NotificationsObjects;
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Japanese:
                NotificationsObjects = japaneseNotificationObjects;
                break;
            case SystemLanguage.Spanish:
                NotificationsObjects= spanishNoticationsObjects;
                break;
            default:
                NotificationsObjects = notificationsObjects;
                break;
        }

        if (NotificationsObjects.Count>0)
            foreach (NotificationObject notiObs in NotificationsObjects)
            {
                notiObs.largeIcon = "large_default";
                notiObs.smallIcon = "small_default";
                notiObs.MyGNM = manager;
                notiObs.SendNotification();
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}

