using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class WheelGame : MonoBehaviour
{
	public static WheelGame Instance { get; set; }

	public bool gameStarted;

	public Transform wheel;
	public AudioSource source;
	public Button spinBtn;
	public Prizes prizes;

	private void OnEnable()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		Start_Game();
	}


	private void OnDisable()
	{
		gameStarted = false;

		StopCoroutine(nameof(Spin_Process));
	}

	private void Start()
	{
		spinBtn.onClick.AddListener(Spin);
	}

	void Start_Game()
	{
		gameStarted = true;

		wheel.rotation = Quaternion.Euler(Vector3.zero);

		spinBtn.interactable = true;
	}

	void Spin()
	{
		Manager.Instance.TrySpin();
		StartCoroutine(nameof(Spin_Process));
	}

	void PlaySound()
	{
		if (source.isPlaying)
		{
			source.Stop();
		}

		source.Play();
	}

	void Game_Over()
	{
		gameStarted = false;
	}

	IEnumerator Spin_Process()
	{
		spinBtn.interactable = false;

		int randomValue = Random.Range(20, 30);

		float timeInterval = 0.1f;

		for (int i = 0; i < randomValue; i++)
		{
			wheel.Rotate(Vector3.forward * 22.5f);

			if (i > Mathf.RoundToInt(randomValue * 0.5f))
			{
				timeInterval = 0.2f;
			}

			if (i > Mathf.RoundToInt(randomValue * 0.85f))
			{
				timeInterval = 0.4f;
			}

			yield return new WaitForSeconds(timeInterval);
		}

		if (Mathf.RoundToInt(wheel.eulerAngles.z % 45) != 0)
		{
			wheel.Rotate(Vector3.forward * 22.5f);
		}

		PlaySound();

		int finalAngle = Mathf.RoundToInt(wheel.eulerAngles.z);

		int prize = GetPrizeCount(finalAngle);

		Manager.Instance.UpdateCoinsCount(prize);

		spinBtn.interactable = true;
	}

	int GetPrizeCount(int angle) => angle switch
	{
		0 => Instance.prizes.prize[0],

		45 => Instance.prizes.prize[1],

		90 => Instance.prizes.prize[2],

		135 => Instance.prizes.prize[3],

		180 => Instance.prizes.prize[4],

		225 => Instance.prizes.prize[5],

		270 => Instance.prizes.prize[6],

		315 => Instance.prizes.prize[7],

		_ => -1
	};

	[System.Serializable]
	public class Prizes
	{
		public int[] prize;
	}
}
