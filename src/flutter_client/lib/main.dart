import 'package:bitsdojo_window/bitsdojo_window.dart';
import 'package:flutter/material.dart';
import 'package:flutter_client/ui/layout/sizes.dart';

import '_app/app_provider.dart';
import '_app/app.dart';
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

  doWhenWindowReady(() {
    final window = appWindow;
    window.minSize = DevBookSizes.minWindowSize;
    window.size = DevBookSizes.startWindowSize;
    window.alignment = Alignment.center;
    window.title = "DevBook";
    window.show();
  });
}
