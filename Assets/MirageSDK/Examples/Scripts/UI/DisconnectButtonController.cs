using MirageSDK.Utils;
using MirageSDK.WalletConnectSharp.Core;
using MirageSDK.WalletConnectSharp.Unity.Events;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MirageSDK.UI
{
	public class DisconnectButtonController : MonoBehaviour
	{
		[SerializeField] private Button _button;
#if !UNITY_WEBGL || UNITY_EDITOR
		private WalletConnectSharp.Unity.WalletConnect WalletConnect =>
			ConnectProvider<WalletConnectSharp.Unity.WalletConnect>.GetConnect();

		private void OnEnable()
		{
			SubscribeEvents();

			_button.onClick.AddListener(OnButtonClick);
		}

		private void OnDisable()
		{
			UnsubscribeEvents();

			_button.onClick.RemoveAllListeners();
		}

		private void SubscribeEvents()
		{
			WalletConnect.SessionStatusUpdated += OnSessionStatusUpdated;
		}

		private void UnsubscribeEvents()
		{
			WalletConnect.SessionStatusUpdated -= OnSessionStatusUpdated;
		}

		private void OnSessionStatusUpdated(WalletConnectTransitionBase transition)
		{
			var status = WalletConnect.Status;
			_button.gameObject.SetActive(status.IsAny(WalletConnectStatus.AnythingConnected));
		}

		private void OnButtonClick()
		{
			WalletConnect.CloseSession().Forget();
		}
#else
		private void Awake()
		{
			gameObject.SetActive(false);
		}
#endif
	}
}