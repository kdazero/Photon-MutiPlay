# Photon Unity多人連線遊戲製作(簡易版)

![gif](/pic/playing.gif)

## 軟體使用教學
### 創建伺服器 & Unity Asset 
1. 前往 [Photon官網](https://www.photonengine.com/zh-TW/Photon) 點擊右上角的登入
![interduce](/pic/002.png)
2. 點擊後若已有帳戶則直接登入，若沒有申請一個

3. 點擊  建立新應用程式 並依下圖設定(網址可空白)  
![interduce](/pic/003.png)
4. 保留此分頁後續在Unity設定時會用到

5. 前往 [Unity Asset Store](https://assetstore.unity.com/packages/tools/network/pun-2-free-119922) 並點擊 Add to My Assets 後登入(請登入跟Unity Hub相同的帳號)
![interduce](/pic/006.png)

6. 出現下圖表示添加成功
![interduce](/pic/0.16.png)

## Unity內部設定教學
### 本篇以3D為例(2/3D皆可使用)

1. 打開專案後在上方工具列中找到 Windows -> Package Manager 並打開
![unity_setting](/pic/009.png)
2. 在 Package Manager 頁面中的左上角把 Packages 的選項改成 My Assets 並找到剛剛添加的 Photon Unity Networking 2 後下載並添加
![unity_setting](/pic/010.png)
3. 添加完後會出現下圖，若沒出現則須重新下載或是你在 Import 時有少載入東西
![unity_setting](/pic/013.png)
4. 返回Photon官網並將剛剛創建的伺服器的應用程式ID整串複製下來後，貼到上圖中的欄位
![unity_setting](/pic/004.png)
5. Setup Project 後的成功畫面

    ![unity_setting](/pic/016.png)

到這裡 Photon 的初步設定就都完成了
***
## Unity Photon 簡易範例
先建立3個場景 Loading / Lobby / GameRoom

![unity](/pic/014.png)

### Loading (連線至伺服器時的載入畫面)
1. 建立簡易UI方便觀看
![unity](/pic/017.png)
2. 創建一隻程式，程式碼如下
    ```c#
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using Photon.Pun; // 使用後才能使用Photon自帶功能
    using UnityEngine.SceneManagement; // 切換場景

    public class ConnectToServer : MonoBehaviourPunCallbacks
    {
        void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep; 
            // sleepTimeout可有可無，僅用於防止載入過久螢幕關閉

            PhotonNetwork.ConnectUsingSettings();
        }
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }
        public override void OnJoinedLobby()
        {
            SceneManager.LoadScene("Lobby"); // 成功連線後切換至大廳
        }
    }
    ```
3. 在場景中建立一個空的GameObject並把上方的程式拖曳至該GameObject中
4. 存檔
### Lobby (遊戲大廳畫面)
#### (可先拉一個文字後返回Loading執行程式若有成功跳轉表示程式 & 伺服器的連線是成功的)
1. 建立簡易UI方便觀看
![unity](/pic/018.png)
文字/按鈕/文字輸入框 皆在 UI -> Legacy 中

    ![unity](/pic/019.png)
2. 創建一隻程式，程式碼如下
    ```c#
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using UnityEngine.UI;
    using Photon.Pun;
    public class CreatAndJoin : MonoBehaviourPunCallbacks
    {
        [SerializeField]private byte maxplayer = 10;
         // 設定每房人數最大上限,0表示沒有上限

        public InputField CreateInput;
        public InputField JoinInput;

        public void CreateRoom()
        {
        PhotonNetwork.CreateRoom(CreateInput.text,new Photon.Realtime.RoomOptions{ MaxPlayers = maxplayer});
        }
        
        public void JoinRoom()
        {
        PhotonNetwork.JoinRoom(JoinInput.text);
        }

        public override void OnJoinedRoom()
        {
        PhotonNetwork.LoadLevel("GameRoom");
        }
    }
    ```
3. 在場景中建立一個空的GameObject並把上方的程式拖曳至該GameObject中並將文字輸入框拖進程式中
    ![unity](/pic/020.png)
4. 設定按鈕的觸發事件，點擊按鈕後下拉找到Onclick後點選 + 號 並將剛剛新增的GameObject拖入
![unity](/pic/022.png)
創建房間點擊 CreateRoom 加入房間則改成 JoinRoom
![unity](/pic/023.png)

5. 存檔
## 遊玩畫面
1. 新增一個資料夾 Resources (一定要用這個名稱，不然Photon抓不到)
2. 在資料夾內放入人物模型等
3. 隨意拉一個場景
4. 角色生成程式
    ```c#
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Networking;
    using Photon.Pun;
    public class Server_Spawn_Player : MonoBehaviourPun
    {
        public GameObject playerPrefab;

        void Start()
        {
            PhotonNetwork.Instantiate(playerPrefab.name,Vector3.zero,Quaternion.identity);
        }
    }
    ```
5. 角色簡易移動
#### 角色需添加 Rigidbody / Photon View / Photon Transform 若有動作則需額外添加 Photon Animator View 

#### Rigidbody 移動用
#### Photon View 多人連線同步器
#### Photon Transform 多人連線角色移動同步
#### Photon Animator View 多人連線角色動作同步
```c#
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Photon.Pun;
    public class playerMove : MonoBehaviour
    {
        PhotonView view;
        Rigidbody rb;
        public float Speed = 10;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            view = GetComponent<PhotonView>();
        }

        void FixedUpdate()
        {
            if (view.IsMine) // 鎖定是自己,如果沒有這行移動時會大家一起動且不會同步
            {
                rb.velocity = new Vector3(Input.GetAxis("Horizontal") * Speed, 0 , Input.GetAxis("Vertical") * Speed);
            }
        }
    }
```
6. 房間內人數統計
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCount : MonoBehaviour
{
    public Text UI_Count;
    [SerializeField]private int PlayerInGame = 0;
   
    void Update()
    {
        UI_Count.text = "" + GameObject.FindGameObjectsWithTag("player").Length;
    }
}
```



