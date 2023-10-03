import 'package:flutter/widgets.dart';
import 'package:flutter_client/app_setups/app_setups_page.dart';
import 'package:flutter_client/dashboard/dashboard_page.dart';
import 'package:flutter_client/startup_profiles/startup_profiles_page.dart';
import 'package:go_router/go_router.dart';

abstract class AppRoutes {
  static const home = '/';
  static const dashboard = '/dashboard';
  static const appSetups = '/appSetups';
  static const startupProfiles = '/startupProfiles';
}

GoRouter createRouter() {
  return GoRouter(
    routes: [
      GoRoute(
        path: AppRoutes.home,
        pageBuilder: (context, state) => NoTransitionPage(
          child: DashboardPage.routeBuilder(context, state),
        ),
      ),
      GoRoute(
        path: AppRoutes.dashboard,
        pageBuilder: (context, state) => NoTransitionPage(
          child: DashboardPage.routeBuilder(context, state),
        ),
      ),
      GoRoute(
        path: AppRoutes.appSetups,
        pageBuilder: (context, state) => NoTransitionPage(
          child: AppSetupsPage.routeBuilder(context, state),
        ),
      ),
      GoRoute(
        path: AppRoutes.startupProfiles,
        pageBuilder: (context, state) => NoTransitionPage(
          child: StartupProfilesPage.routeBuilder(context, state),
        ),
      ),
    ],
    observers: [RedirectToHomeObserver()],
  );
}

class RedirectToHomeObserver extends NavigatorObserver {
  @override
  void didPush(Route<dynamic> route, Route<dynamic>? previousRoute) {
    super.didPush(route, previousRoute);

    if (previousRoute == null && route.settings.name != AppRoutes.home) {
      WidgetsBinding.instance.addPostFrameCallback((_) {
        final context = route.navigator!.context;
        GoRouter.of(context).go(AppRoutes.home);
      });
    }
  }
}
