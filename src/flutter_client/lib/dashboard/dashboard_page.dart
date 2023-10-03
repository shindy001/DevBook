import 'package:flutter/material.dart';
import 'package:flutter_client/ui/widgets/page_with_navigation.dart';

class DashboardPage extends StatelessWidget {
  const DashboardPage({super.key});

  factory DashboardPage.routeBuilder(_, __) {
    return const DashboardPage(
      key: Key('dashboard'),
    );
  }

  @override
  Widget build(BuildContext context) {
    return const PageWithNavigation(
      activeSideMenuItemName: 'Dashboard',
      child: Text('Hello Dashboard'),
    );
  }
}
