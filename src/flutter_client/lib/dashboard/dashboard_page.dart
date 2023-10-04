import 'package:flutter/material.dart';
import 'package:flutter_client/ui/theme/dev_book_spacing.dart';
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
    return PageWithNavigation(
      activeSideMenuItemName: 'Dashboard',
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Padding(
            padding: const EdgeInsets.all(DevBookSpacing.sm),
            child: Text(
              'Dashboard',
              style: Theme.of(context).textTheme.headlineLarge,
            ),
          )
        ],
      ),
    );
  }
}
