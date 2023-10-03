import 'package:flutter/material.dart';
import 'package:flutter_client/ui/widgets/page_with_navigation.dart';

class AppSetupsPage extends StatelessWidget {
  const AppSetupsPage({super.key});

  factory AppSetupsPage.routeBuilder(_, __) {
    return const AppSetupsPage(
      key: Key('appSetups'),
    );
  }

  @override
  Widget build(BuildContext context) {
    return const PageWithNavigation(
      activeSideMenuItemName: 'App Setups',
      child: Text('Hello App Setups'),
    );
  }
}
