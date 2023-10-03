import 'package:flutter/material.dart';
import 'package:flutter_client/ui/widgets/page_with_navigation.dart';

class StartupProfilesPage extends StatelessWidget {
  const StartupProfilesPage({super.key});

  factory StartupProfilesPage.routeBuilder(_, __) {
    return const StartupProfilesPage(
      key: Key('startupProfiles'),
    );
  }

  @override
  Widget build(BuildContext context) {
    return const PageWithNavigation(
      activeSideMenuItemName: 'Startup Profiles',
      child: Text('Hello Startup Profiles'),
    );
  }
}
