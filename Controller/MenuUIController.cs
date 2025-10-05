using System;
using System.Collections.Generic;
using System.Linq;
using DBH.Attributes;
using DBH.Base;
using DBH.Input.api.Extending;
using DBH.Input.api.Keys;
using DBH.UI.Input;
using DBH.UI.Menu.MenuParent;
using Sirenix.OdinInspector;
using UnityEngine;
using Vault;
using Vault.BetterCoroutine;

namespace DBH.UI.Controller {
    [Attributes.Controller]
    public class MenuUIController : DBHMono, IMenuUIController {
        [Grab]
        private List<IMenuInteractionChanged> menuInteractionChangeds;

        [Grab]
        private AbortInput abortReleaseInput;

        [Grab]
        private ConfirmButtonInput confirmButtonReleaseInput;

        [Grab]
        private ConfirmProgressInput confirmProgressInput;

        [Grab]
        private IncDecInput incDecInput;

        [Grab]
        private RawDirectionInput rawDirectionInput;

        [SerializeField]
        private List<AudioSourceDto> audioSources;


        [ReadOnly]
        [ShowInInspector]
        private bool isEnabled;

        [ReadOnly]
        [ShowInInspector]
        public HashSet<MenuParent> CurrentlyOpenMenus { get; } = new();

        [ReadOnly]
        [ShowInInspector]
        public MenuParent CurrentMenu { get; private set; }

        public event IMenuUIController.ClosedMenu OnCloseMenu;

        [PostConstruct]
        private void BindInput() {
            confirmButtonReleaseInput.OnConfirmButtonPressed += Confirm;
            abortReleaseInput.OnAbortPressed += AbortRelease;
            incDecInput.OnIncDecPressed += OnIncDecPressed;
            rawDirectionInput.OnRawInput += RawUiInput;
            confirmProgressInput.OnConfirmHoldAborted += ConfirmProgressAborted;
            confirmProgressInput.OnConfirmProgress += ConfirmProgress;
            confirmProgressInput.OnConfirmCompleted += ConfirmProgressCompleted;
        }

        public void GoBack() {
            if (CurrentlyOpenMenus.Count < 1) return;
            var lastMenuParent = CurrentlyOpenMenus.Last();
            lastMenuParent.DeActivateMenu();
            lastMenuParent.Close();
            OnCloseMenu?.Invoke(lastMenuParent);
            CurrentlyOpenMenus.RemoveLastItem();
            CurrentlyOpenMenus.ForEach(parent => parent.Open());
            if (CurrentlyOpenMenus.IsEmpty()) {
                Close();
            } else {
                AddMenuAndChange(CurrentlyOpenMenus.Last());
            }
        }

        public void Close() {
            foreach (var currentlyOpenMenu in CurrentlyOpenMenus) {
                currentlyOpenMenu.DeActivateMenu();
                currentlyOpenMenu.Close();
                OnCloseMenu?.Invoke(currentlyOpenMenu);
            }

            CurrentlyOpenMenus.Clear();
            DisableMenuInteraction();
        }

        public void OpenOnlyFirst() {
            var menuParent = CurrentlyOpenMenus.First();
            Close();
            AddMenuAndChange(menuParent);
        }

        public void AddMenuAndChange(MenuParent toChangeMenuParent) {
            EnableMenuInteraction();
            if (!CurrentlyOpenMenus.IsEmpty()) {
                CurrentlyOpenMenus.ForEach(m => m.DeActivateMenu());
            }

            CurrentlyOpenMenus.Add(toChangeMenuParent);
            toChangeMenuParent.ActivateMenu();
            CurrentMenu = toChangeMenuParent;
        }

        public void ChangeCurrentMenu(MenuParent toChangeMenuParent) {
            if (!CurrentlyOpenMenus.IsEmpty()) {
                CurrentlyOpenMenus.ForEach(m => m.DeActivateMenu());
            }

            toChangeMenuParent.ActivateMenu();
            if (toChangeMenuParent.MenuPointCount() == 0) {
                toChangeMenuParent.DeActivateMenu();
                CurrentlyOpenMenus.Last().ActivateMenu();
            } else {
                CurrentlyOpenMenus.RemoveLastItem();
                CurrentlyOpenMenus.Add(toChangeMenuParent);
                CurrentMenu = toChangeMenuParent;
            }
        }

        public void NavigateTo(MenuParent menuParent) {
            if (CurrentMenu == menuParent) return;
            while (CurrentMenu != menuParent) {
                GoBack();
            }
        }

        public void OpenMenus() {
            CurrentlyOpenMenus.ForEach(m => m.Open());
        }

        public void HideMenus() {
            CurrentlyOpenMenus.ForEach(m => m.Close());
        }

        public void OpenOnlyLast() {
            CurrentlyOpenMenus.Last().Open();
        }

        [ContextMenu("enable")]
        public void EnableMenuInteraction() {
            if (isEnabled) return;
            isEnabled = true;
            menuInteractionChangeds.OrderBy(changed => changed.Order())
                .ForEach(changed => {
                    changed.BeforeEnabled();
                    AsyncRuntime.WaitForEndOfFrame(changed.Enabled);
                });
        }

        public void DisableMenuInteraction() {
            menuInteractionChangeds.OrderBy(changed => changed.Order())
                .ForEach(changed => changed.Disabled());
            isEnabled = false;
        }

        private void OnIncDecPressed(Direction direction) {
            var couldMove = direction switch {
                Direction.VerticalInc => CurrentMenu.IncreaseVertical(),
                Direction.VerticalDec => CurrentMenu.DecreaseVertical(),
                Direction.HorizontalDec => CurrentMenu.DecreaseHorizontal(),
                Direction.HorizontalInc => CurrentMenu.IncreaseHorizontal(),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
            if (couldMove) {
                audioSources.FindOptional(dto => dto.MenuActionType == MenuActionType.CursorMove)
                    .IfPresent(dto => dto.AudioSource.Play());
            }
        }

        [Button]
        private void CreateKeyBindings() {
            var inputController = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IInputController>().First();
            inputController.AddButton(new InputKeys("UI", "Confirm", Lists.Of(KeyCode.Return, KeyCode.F)));
            inputController.AddButton(new InputKeys("UI", "Abort", Lists.Of(KeyCode.Escape, KeyCode.Backspace)));
            inputController.AddButton(new InputKeys("UI", "ConfirmProgress", Lists.Of(KeyCode.F)));
            inputController.AddButton(new DirectionKeys("UI", "IncDec"));
            inputController.AddButton(new DirectionKeys("UI", "RawDirection"));
        }

        private void Confirm() {
            CurrentMenu.Commit(new AudioPlayerDto(audioSources.Find(dto => dto.MenuActionType == MenuActionType.Commit).AudioSource));
        }

        private void ConfirmProgress(int progress) {
            CurrentMenu.CommitProgress(progress);
        }

        private void ConfirmProgressCompleted() {
            CurrentMenu.CommitProgressCompleted();
        }

        private void ConfirmProgressAborted() {
            CurrentMenu.CommitProgressAborted();
        }

        private void AbortRelease() {
            CurrentMenu.Abort(new AudioPlayerDto(audioSources.Find(dto => dto.MenuActionType == MenuActionType.Abort).AudioSource));
        }

        private void RawUiInput(Vector2 vector2) {
            CurrentMenu.InputRaw(vector2);
        }
    }
}