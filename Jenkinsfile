pipeline {
  agent any
  triggers {
    pollSCM('H/2 * * * *')
  }
  environment {
    ACR_URL = 'amircontainerregistry.azurecr.io'
    CONTAINER_IMAGE_NAME = ''
  }
  parameters {
    choice(name: 'BuildOptions', choices: ['Only on app files changes', 'Force', 'Skip'], description: 'Build options')
    string(name: 'ManualDeployImage', defaultValue: '', description: 'Manually deploy image name (leave blank to skip deploy)')
  }
  stages {
    stage("build") {
      when {
        expression {
          params.BuildOptions == 'Force' /*||
          (params.BuildOptions == 'Only on app files changes' && changeset "TestApp*/**")*/
        }
      }
      steps {      
        env.CONTAINER_IMAGE_NAME = "${ACR_URL}/stable/restapi:${BUILD_TIMESTAMP}"
        echo "building image ${CONTAINER_IMAGE_NAME}"
        /*script {
          withCredentials([usernamePassword(credentialsId: 'ACR', usernameVariable: 'ACR_USER', passwordVariable: 'ACR_PASSWORD')]) {
            sh "docker login -u $ACR_USER -p $ACR_PASSWORD https://${ACR_URL}"
            def image = docker.build "${CONTAINER_IMAGE_NAME}"
            image.push()
          }
        }*/
      }
    }
    stage("deploy") {
      steps {
        script {
          if (params.ManualDeployImage != '')
            env.CONTAINER_IMAGE_NAME = params.ManualDeployImage
          
          echo "deploying image ${CONTAINER_IMAGE_NAME}"
          
          /*kubernetesDeploy(
              configs: 'azure-testapp.yaml',
              kubeconfigId: 'KubeConfig',
              enableConfigSubstitution: true
              )   */        
        }
      }
    }
  }
}
