using UnityEngine;
using System.Collections;

public class Utils : MonoBehaviour
{
    public static bool isTouchDevice=(
                                     Application.platform==RuntimePlatform.Android
                                     ||
                                     Application.platform==RuntimePlatform.BB10Player
                                     ||
                                     Application.platform==RuntimePlatform.IPhonePlayer
                                     ||
                                     Application.platform==RuntimePlatform.WP8Player
                                    );

    public static bool isWebPlayer=(
                                    Application.platform==RuntimePlatform.OSXWebPlayer
                                    ||
                                    Application.platform==RuntimePlatform.WindowsWebPlayer
                                   );
}
