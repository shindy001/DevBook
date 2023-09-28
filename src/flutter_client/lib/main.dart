import 'package:bitsdojo_window/bitsdojo_window.dart';
import 'package:flutter/material.dart';

import '_app/app_provider.dart';
import '_app/widgets/app.dart';
import 'settings/settings_controller.dart';
import 'settings/settings_service.dart';

void main() async {
  final settingsController = Settings(SettingsService());
  await settingsController.loadSettings();

  runApp(
    AppProvider(
      services: [
        settingsController,
      ],
      child: const App(),
    ),
  );
}
