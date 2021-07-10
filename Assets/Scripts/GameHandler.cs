using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

namespace PickBonus { 
    public class GameHandler : MonoBehaviour {
        [Header("Buttons")]
        public Button playButton;
        public Button increaseButton;
        public Button decreaseButton;
        public Button firstButton;
        public Button secondButton;
        public Button thirdButton;
        public Button fourthButton;

        [Header("Text")]
        public Text currentDenominationText;
        public Text currentBalanceText;
        public Text currentWonInGameText;

        [Header("Game Objects")]
        public GameObject chestContainer;
        public GameObject chestPrefab;
        public GameObject explosion;
        public GameObject gameoverScreen;
        public GameObject moneyTarget;

        [Header("Animator")]
        public Animator explosionAnimation;

        //is game on
        private bool isGame;

        //in game main variables
        private float[] denominationList = new float[] {0.25f, 0.50f, 1.00f, 5.00f};
        private int currentDenominationIndex = 0;
        private double currentBalance;
        private double lastGameWin;

        //prefab chest list
        private List<GameObject> chestList;
        private List<int> clickedChests;

        //Multipliers
        private int[] fiftyChance = new int[] { 0 };
        private int[] thirtyChance = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int[] fifteenChance = new int[] { 12, 16, 24, 32, 48, 64 };
        private int[] fiveChance = new int[] { 100, 200, 300, 400, 500 };

        //amount of money won in round
        private List<double> amountInChests;

        //animator
        private Animator openAnimation;
        private Animator wiggleAnimation;
        private List<int> notOpenedChests;

        //coins to move
        private List<GameObject> coinMoveList;

        private void Awake()
        {
            //set default
            chestList = new List<GameObject>();
            clickedChests = new List<int>();
            amountInChests = new List<double>();
            notOpenedChests = new List<int>();
            coinMoveList = new List<GameObject>();

            currentBalance = 10.00f;
            lastGameWin = 0.0f;

            isGame = false;
            FindObjectOfType<AudioManager>().Play("Theme");

        }
    
        void Start()
        {
            openAnimation = GetComponent<Animator>();

            currentDenominationText.text = denominationList[currentDenominationIndex].ToString("C", CultureInfo.CurrentCulture);

            // setting up chests
            for (int i = 0; i < 9; i++)
            {
                GameObject childChest = chestContainer.transform.GetChild(i).gameObject;
                chestList.Add(childChest);
                notOpenedChests.Add(i);
            }
        }

        void Update()
        {
            if (coinMoveList.Count > 0) {
                MoveCoinToTarget();
            }
        }

        private void MoveCoinToTarget() {
            for(int i = 0; i < coinMoveList.Count; ) {
                if (!isGame || coinMoveList[i].transform.position == moneyTarget.transform.position) {
                    coinMoveList[i].SetActive(false);
                    coinMoveList[i].transform.localPosition = Vector3.zero;
                    coinMoveList.Remove(coinMoveList[i]);
                }
                else {
                    coinMoveList[i].transform.position = Vector3.MoveTowards(coinMoveList[i].transform.position,
                        moneyTarget.transform.position, Time.deltaTime * 300f);
                    i++;
                }
            }
        }

        private void WaitBeforeWiggle() {
            if (!isGame) return;

            int randomBoxIndex = notOpenedChests[Random.Range(0, notOpenedChests.Count)];

            wiggleAnimation = chestList[randomBoxIndex].GetComponent<Animator>();
            wiggleAnimation.SetBool("isWiggle", true);

            StartCoroutine(StopAnimation(wiggleAnimation));
        }

        private IEnumerator StopAnimation(Animator wiggleAnimation)
        {
            yield return new WaitForSeconds(3);
            wiggleAnimation.SetBool("isWiggle", false);
        }

        public void IncreaseDenomination()
        {
            if (currentDenominationIndex == 3) return;

            currentDenominationIndex++;
            currentDenominationText.text = denominationList[currentDenominationIndex].ToString("C", CultureInfo.CurrentCulture);
            EnoughBalance();
        }
   
        public void DecreaseDenomination()
        {
            if (currentDenominationIndex == 0) return;

            currentDenominationIndex--;
            currentDenominationText.text = denominationList[currentDenominationIndex].ToString("C", CultureInfo.CurrentCulture);
            EnoughBalance();
        }

        public void BetAmountOptionOne() {
            currentDenominationIndex = 0;
            currentDenominationText.text = (0.25).ToString("C", CultureInfo.CurrentCulture);
            EnoughBalance();
        }

        public void BetAmountOptionTwo() {
            currentDenominationIndex = 1;
            currentDenominationText.text = (0.50).ToString("C", CultureInfo.CurrentCulture);
            EnoughBalance();
        }
   
        public void BetAmountOptionThree() {
            currentDenominationIndex = 2;
            currentDenominationText.text = (1.00).ToString("C", CultureInfo.CurrentCulture);
            EnoughBalance();
        }
    
        public void BetAmountOptionFour() {
            currentDenominationIndex = 3;
            currentDenominationText.text = (5.00).ToString("C", CultureInfo.CurrentCulture);
            EnoughBalance();
        }

        private void EnoughBalance() {
            if (currentBalance == 0.0f) {
                FindObjectOfType<AudioManager>().Stop("Theme");
                FindObjectOfType<AudioManager>().Play("Gameover");
                gameoverScreen.SetActive(true);
                return;
            }
            if (currentBalance < denominationList[currentDenominationIndex])
            {
                playButton.interactable = false;
            }
            else
            {
                playButton.interactable = true;
            }
        }

        public void ChestClicked(GameObject chest) {
            if (!isGame) return;

            int index = chest.transform.GetSiblingIndex();
            if (clickedChests.Contains(index)) return;

            //add to clicked list
            clickedChests.Add(index);

            //open chest with money if any
            GameObject empty = chest.transform.GetChild(1).gameObject;
            GameObject money = chest.transform.GetChild(2).gameObject;

            //instant loss
            if (amountInChests.Count == 0) {
                isGame = false;
                CancelInvoke();

                empty.SetActive(true);
                money.SetActive(false);

                explosion.SetActive(true);
                explosionAnimation.SetBool("isGameOver", true);

                //get Audio Manager sense its gonna be played a small amount of times
                FindObjectOfType<AudioManager>().Play("Explosion");

                //visually reset
                foreach (GameObject item in chestList)
                {
                    item.GetComponent<Animator>().SetBool("isOpen", false);
                    item.GetComponent<Animator>().SetBool("isHover", false);
                    item.transform.GetChild(4).gameObject.SetActive(false);
                }

                //reset lists
                clickedChests.Clear();
                amountInChests.Clear();
                for (int i = 0; i < 9; i++)
                {
                    notOpenedChests.Add(i);
                }
                playButton.interactable = true;
                increaseButton.interactable = true;
                decreaseButton.interactable = true;
                firstButton.interactable = true;
                secondButton.interactable = true;
                thirdButton.interactable = true;
                fourthButton.interactable = true;

                currentBalance += lastGameWin;

                //show current balance
                currentBalanceText.text = currentBalance.ToString("C", CultureInfo.CurrentCulture);
                currentWonInGameText.text = (0.0f).ToString("C", CultureInfo.CurrentCulture);

                EnoughBalance();

                PrintInformation();

                return;
            }
            //random amount from list
            else {
                empty.SetActive(false);
                money.SetActive(true);

                int randomIndexAmount = Random.Range(0, amountInChests.Count);

                //add to total (last game win)
                lastGameWin += amountInChests[randomIndexAmount];

                GameObject chestAmount = chest.transform.GetChild(4).gameObject;
                chestAmount.SetActive(true);
                chestAmount.GetComponent<Text>().text = amountInChests[randomIndexAmount].ToString("C", CultureInfo.CurrentCulture);

                currentWonInGameText.text = lastGameWin.ToString("C", CultureInfo.CurrentCulture);

                //show the amount in chest
                openAnimation = chest.GetComponent<Animator>();
                openAnimation.SetBool("isOpen", true);
                FindObjectOfType<AudioManager>().Play("Coin");

                //flying coin
                GameObject coin = chest.transform.GetChild(5).gameObject;
                coin.SetActive(true);
                coinMoveList.Add(coin);


                //remove from list
                amountInChests.RemoveAt(randomIndexAmount);

            }

            //remove potential wiggle chest
            notOpenedChests.RemoveAt(notOpenedChests.IndexOf(index));
        }
    
        public void HoverOverChest() {
            if (!isGame) return;
            FindObjectOfType<AudioManager>().Play("Hover");
        }

        public void HoverOverAnimation(GameObject chest) {
            if (!isGame) return;
            if (!chest.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("isHover")) {
                chest.GetComponent<Animator>().SetBool("isHover", true);
            }
        }
    
        public void UnHoverOverAnimation(GameObject chest) {
            if (!isGame) return;

            chest.GetComponent<Animator>().SetBool("isHover", false);
        }

        public void ResetGame()
        {
            chestList.Clear();
            clickedChests.Clear();
            amountInChests.Clear();
            notOpenedChests.Clear();

            //set default
            currentBalance = 10.00f;
            currentBalanceText.text = currentBalance.ToString("C", CultureInfo.CurrentCulture);

            lastGameWin = 0.0f;
            currentWonInGameText.text = lastGameWin.ToString("C", CultureInfo.CurrentCulture);

            isGame = false;
        
            currentDenominationText.text = denominationList[currentDenominationIndex].ToString("C", CultureInfo.CurrentCulture);

            // setting up chests
            for (int i = 0; i < 9; i++)
            {
                GameObject childChest = chestContainer.transform.GetChild(i).gameObject;
                chestList.Add(childChest);
                notOpenedChests.Add(i);
            }

            FindObjectOfType<AudioManager>().Play("Theme");
            FindObjectOfType<AudioManager>().Stop("Gameover");

            //show restart screen
            gameoverScreen.SetActive(false);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void StartGame() {
            //turn  off explosion
            if (explosion.gameObject.activeSelf) {
                explosion.SetActive(false);
                explosionAnimation.SetBool("isGameOver", false);
            }

            //disabled when game started
            playButton.interactable = false;
            increaseButton.interactable = false;
            decreaseButton.interactable = false;
            firstButton.interactable = false;
            secondButton.interactable = false;
            thirdButton.interactable = false;
            fourthButton.interactable = false;

            lastGameWin = 0.0f;

            //subtract Denomination from Current Balance
            currentBalance -= denominationList[currentDenominationIndex];

            //show current balance
            currentBalanceText.text = currentBalance.ToString("C", CultureInfo.CurrentCulture);

            GetMultipliers(denominationList[currentDenominationIndex]);

            InvokeRepeating("WaitBeforeWiggle", 1.0f, 3.0f);

            isGame = true;
            PrintInformation();
        }

        private void GetMultipliers(float denomination) {
            int probabilityNumber = GetRandomValue();

            float winAmount = 0.0f;

            if (probabilityNumber == 50){
                winAmount = denomination * fiftyChance[0];
            }
            else if (probabilityNumber == 30) {

                winAmount = denomination * thirtyChance[Random.Range(0, thirtyChance.Length)];
            }
            else if (probabilityNumber == 15){
                winAmount = denomination * fifteenChance[Random.Range(0, fifteenChance.Length)];
            }
            else if (probabilityNumber == 5){
                winAmount = denomination * fiveChance[Random.Range(0, fiveChance.Length)];
            }

            GetTotalSplit(winAmount);                
        }

        private void GetTotalSplit(float winAmount) {
            if (winAmount == 0.0) return;

            //if less than 1.00
            if (winAmount < 1.00) {
                amountInChests.Add(winAmount);
            }

            //split total win if greater 15.00
            else if (winAmount > 15.00) {
                double cents = winAmount - (int)winAmount;

                // has cents
                if (cents != 0) {
                    //divide by 7
                    int smallAmounts = NearestDivisor((int)winAmount, 7);

                    //not divisable 
                    if (smallAmounts != (int)winAmount) {
                        for (int i = 0; i < 7; i++) {
                            amountInChests.Add(smallAmounts / 7);
                        }

                        int difference = (int)winAmount - smallAmounts;
                        int index = 0;
                        while (difference != 0) {
                            amountInChests[index]++;
                            difference--;
                            index++;

                            if (index == 7) {
                                index = 0;
                            }
                        }

                    }
                    //divisable
                    else {
                        for (int i = 0; i < 7; i++) {
                            amountInChests.Add(smallAmounts / 7);
                        }
                    }

                    amountInChests.Add(cents);
                }

                //doesn't have cents
                else {
                    //divide by 8
                    int smallAmounts = NearestDivisor((int)winAmount, 8);

                    //not divisable 
                    if (smallAmounts != (int)winAmount) {
                        for (int i = 0; i < 8; i++) {
                            amountInChests.Add(smallAmounts / 8);
                        }

                        int difference = (int)winAmount - smallAmounts;
                        int index = 0;
                        while (difference != 0) {
                            amountInChests[index]++;
                            difference--;
                            index++;

                            if (index == 8)
                            {
                                index = 0;
                            }
                        }
                    }
                    //divisable
                    else {
                        for (int i = 0; i < 8; i++) {
                            amountInChests.Add(smallAmounts / 8);
                        }
                    }
                }
            }

            // if in the range of 1.00 - 15.00
            else {
                amountInChests.Add((int)winAmount);
                double cents = winAmount - (int)winAmount;
                if (cents != 0.00) { 
                    amountInChests.Add(cents);
                }
            }

        }

        private int NearestDivisor(int amount, int divisor) {
            int result = amount;
            while (result % divisor != 0) {
                result--;
            }
            return result;
        }

        private int GetRandomValue() {
            float rand = Mathf.Round(Random.value * 100.0f) * 0.01f;
            if (rand <= .05f)
                return 5;
            if (rand <= .15f)
                return 15;
            if (rand <= .30f)
                return 30;
            return 50;
        }

        private void PrintInformation() {
            Debug.Log("-----------------------------------------------");
            Debug.Log("Current Denomination: " + denominationList[currentDenominationIndex]);
            Debug.Log("Current Balance: " + currentBalance);
            Debug.Log("Last Game Win: " + lastGameWin);
            Debug.Log("-----------------------------------------------");
        }
    }
}
