# Blockforge

Мобильный Unity clicker в стиле блочного фэнтези.

## Быстрый запуск

- Unity 2022.3.40f1
- Android Build Support
- Откройте проект через Unity Hub.
- Сцена: Assets/Scenes/Main.unity

## Автоматическая сборка APK

Workflow: .github/workflows/android-build.yml

После push в GitHub Actions собирается Android APK и сохраняется как artifact.

Для GameCI нужен секрет GitHub:
- UNITY_LICENSE — содержимое Unity license file

Также можно запускать workflow вручную через Actions → Build Android APK → Run workflow.

