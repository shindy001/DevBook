import 'package:flutter/material.dart';

import '../settings/settings_controller.dart';

class Dashboard extends StatelessWidget {
  const Dashboard({
    super.key,
    required this.title,
    required this.settings,
  });

  final String title;
  final Settings settings;

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: <Widget>[
          const Text(
            'Hello',
          ),
          Padding(
            padding: const EdgeInsets.all(16),
            child: DropdownButton<ThemeMode>(
              value: settings.themeMode,
              onChanged: settings.updateThemeMode,
              items: const [
                DropdownMenuItem(
                  value: ThemeMode.system,
                  child: Text('System Theme'),
                ),
                DropdownMenuItem(
                  value: ThemeMode.light,
                  child: Text('Light Theme'),
                ),
                DropdownMenuItem(
                  value: ThemeMode.dark,
                  child: Text('Dark Theme'),
                )
              ],
            ),
          ),
        ],
      ),
    );
  }
}
