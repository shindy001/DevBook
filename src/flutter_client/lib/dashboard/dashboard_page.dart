import 'package:flutter/material.dart';
import 'package:flutter_client/ui/widgets/page_with_navigation.dart';

class Dashboard extends StatelessWidget {
  const Dashboard({super.key});

  factory Dashboard.routeBuilder(_, __) {
    return const Dashboard(
      key: Key('dashboard'),
    );
  }

  @override
  Widget build(BuildContext context) {
    return const PageWithNavigation(
      activeSideMenuItemName: 'Dashboard',
      child: Text(
        'Hello Dashboard',
      ),
    );
  }
}
